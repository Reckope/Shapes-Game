/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is the core script for peds such as the Player, enemies, allies etc.
* All peds inherit this script, where they can then access it's components,
* methods, properties and their State Machine.  
* Derived Ped > Ped (here) > Statemachine > State > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StateMachine))]
public class Ped : MonoBehaviour
{
	// ============================================================
	// Everything a healthy ped needs.
	// ============================================================

	// Classes
	protected StateMachine stateMachine;

	// Components
	public Rigidbody2D Rigidbody2D;
	public Collider2D Collider2D;
	public Animator Animator;

	// GameObjects / Transform
	public LayerMask whatIsGround;
	public Transform areaColliders;
	public Transform groundCheck, leftCheck, rightCheck, topCheck, morphIntoBoxCheck;

	// Global Variables
	public string Name { get; protected set; }
	public float Speed { get; set; }
	public float JumpForce { get; set; }
	private float groundCheckRadius = 0.1f, blockCheckRadius = 3.5f;
	private Quaternion rotation;
	private string _sound;
	private float _movementDirection;

	// Detect collisions around the ped to prevent morphing in tight spaces.
	public bool CollidedLeft { get { return Physics2D.OverlapCircle (leftCheck.position, groundCheckRadius, whatIsGround); } }
	public bool CollidedRight { get { return Physics2D.OverlapCircle (rightCheck.position, groundCheckRadius, whatIsGround); } }
	public bool CollidedTop { get { return Physics2D.OverlapCircle (topCheck.position, groundCheckRadius * 3, whatIsGround); } }
	public bool IsGrounded { get { return Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround); } }
	public bool CantMorphIntoBlock { get { return Physics2D.OverlapCircle (morphIntoBoxCheck.position, blockCheckRadius, whatIsGround); } }

	public bool HasMorphed { get; set; }
	public bool HasJumped { get; protected set; }
	public bool IsAbleToMove { get; set; }
	public bool IsAbleToJump {get; set; }
	public bool HasHitTheGround {get; set; }

	// *** JOE. If multiple peds are acting the same, create a constructor and use:
	// Ped player = new ped(name, speed etc) ***

	// ============================================================
	// MonoBehaviour methods.
	// ============================================================

	protected virtual void Awake()
	{
		Rigidbody2D = GetComponent<Rigidbody2D>();
		Collider2D = GetComponent<Collider2D>();
		Animator = GetComponent<Animator>();
		stateMachine = GetComponent<StateMachine>();
	}

	protected virtual void Start()
	{
		Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
		rotation = areaColliders.transform.rotation;
	}

	protected virtual void Update()
	{
		stateMachine.UpdateState();
		Debug.Log(stateMachine.GetCurrentState);
		FlipSprite();
		UpdateAirbornAnim();
		areaColliders.transform.rotation = rotation;
	}

	protected virtual void FixedUpdate()
	{
		stateMachine.FixedUpdateState();
		Walk();
		if(HasJumped) { Jump(); }
	}

	protected virtual void LateUpdate()
	{
		stateMachine.LateUpdateState();
	}

	// ============================================================
	// Properties with logic.
	// ============================================================

	public string Sound { get { return _sound; } set { _sound = value; } }

	// Set Movement Direction as a property so we only enter the
	// walking & idle states once, and not every frame. 
	public float MovementDirection 
	{ 
		get { return _movementDirection; }
		set {
			if(_movementDirection == value)
			{
				return;
			}
			_movementDirection = value;
			if(!HasMorphed)
			{
				if(_movementDirection != 0)
				{
					stateMachine.SetState(new WalkingState(stateMachine, this));
				}
				else
				{
					stateMachine.SetState(new IdleState(stateMachine, this));
				}
			}
		}
	}

	// ============================================================
	// Ped Basic Tasks & Movement
	// ============================================================

	public void Jump()
	{
		if(IsAbleToJump)
		{
			Animator.SetTrigger("takeOff");
			Rigidbody2D.velocity = Vector2.up * JumpForce;
			HasJumped = false;
		}
	}

	public void Walk()
	{
		if(IsAbleToMove)
		{
			Rigidbody2D.velocity = new Vector2(MovementDirection * Speed, Rigidbody2D.velocity.y);
		}
	}

	private void FlipSprite()
	{
		if(Rigidbody2D.velocity.x > 0 && !HasMorphed)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		else if(Rigidbody2D.velocity.x < 0 && !HasMorphed)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
	}

	private void UpdateAirbornAnim()
	{
		if(IsGrounded)
		{
			Animator.SetBool("isAirborn", false);
		}
		else
		{
			Animator.SetBool("isAirborn", true);
		}
	}

	public void TransitionAfterExitingState()
	{
		if(MovementDirection == 0)
		{
			stateMachine.SetState(new IdleState(stateMachine, this));
		}
		else
		{
			stateMachine.SetState(new WalkingState(stateMachine, this));
		}
	}

	// ============================================================
	// Detect collisions
	// ============================================================

	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && HasMorphed)
		{
			HasHitTheGround = true;
		}
	}

	private void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			HasHitTheGround = false;
		}
	}
}