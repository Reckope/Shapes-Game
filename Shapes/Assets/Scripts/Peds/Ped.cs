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

using System;
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

	protected enum MorphStates
	{
		Ball,
		Block,
		HorizontalShield,
		VerticalShield
	};

	MorphStates morphStates;

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
	public float GroundCheckRadius { get; set; }
	[SerializeField]
	private float blockCheckRadius = 3.5f;
	private Quaternion rotation;
	private string _sound;
	private float _movementDirection;

	// Detect collisions around the ped to prevent morphing in tight spaces.
	public bool CollidedLeft { get { return Physics2D.OverlapCircle (leftCheck.position, GroundCheckRadius, whatIsGround); } }
	public bool CollidedRight { get { return Physics2D.OverlapCircle (rightCheck.position, GroundCheckRadius, whatIsGround); } }
	public bool CollidedTop { get { return Physics2D.OverlapCircle (topCheck.position, GroundCheckRadius, whatIsGround); } }
	public bool IsGrounded { get { return Physics2D.OverlapCircle (groundCheck.position, GroundCheckRadius, whatIsGround); } }
	public bool CantMorphIntoBlock { get { return Physics2D.OverlapCircle (morphIntoBoxCheck.position, blockCheckRadius, whatIsGround); } }

	public bool HasMorphed { get; set; }
	public bool HasJumped { get; protected set; }
	public bool IsAbleToMove { get; set; }
	public bool IsAbleToJump {get; set; }
	public bool HasHitTheGroundWhileMorphed {get; set; }

	// Player Input
	public bool MorphToBallInput { get; set; }
	public bool MorphToBlockInput { get; set; }
	public bool MorphToHorizontalShieldInput { get; set; }
	public bool MorphToVerticalShieldInput { get; set; }

	// *** JOE. If multiple peds are acting the same, create a constructor and use:
	// Ped player = new ped(name, speed etc) ***

	// ============================================================
	// MonoBehaviour methods
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
		if(HasJumped)
		{ 
			Jump(); 
		}
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
	// Ped Basic Tasks, States & Movement
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

	public void ExitMorphState()
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

	// Any ped can call this method to set a state, and not have to worry about
	// adding additional functionality to only call the state for one frame. 
	protected void SetMorphState(MorphStates state)
	{
		float blockUpwardForce = 180f;

		if(HasMorphed)
		{
			return;
		}
		else
		{
			switch(state)
			{
				case MorphStates.Ball: 
				stateMachine.SetState(new MorphIntoBallState(stateMachine, this));
				break;
				case MorphStates.Block:
				Rigidbody2D.AddForce(transform.up * blockUpwardForce);
				stateMachine.SetState(new MorphIntoBlockState(stateMachine, this));
				break;
				case MorphStates.HorizontalShield:
				stateMachine.SetState(new MorphIntoHorizontalShieldState(stateMachine, this));
				break;
				case MorphStates.VerticalShield:
				stateMachine.SetState(new MorphIntoVerticalShieldState(stateMachine, this));
				break;
			}
		}
	}

	// ============================================================
	// Detect collisions 
	// Collisions are detected here, then state classes can reference various bools to determine what to do.
	// ============================================================

	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && HasMorphed)
		{
			HasHitTheGroundWhileMorphed = true;
		}
	}

	private void OnCollisionExit2D(Collision2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && HasMorphed)
		{
			HasHitTheGroundWhileMorphed = false;
		}
	}
}