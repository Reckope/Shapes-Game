/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is the core script for peds such as the Player, enemies, allies etc.
* All peds inherit this script, where they can then access it's components
* and the state machine.  
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
	// Classes
	protected StateMachine stateMachine;

	// Components
	public Rigidbody2D Rigidbody2D;
	public Collider2D Collider2D;
	public Animator Animator;

	// GameObjects / Transform
	public LayerMask whatIsGround;
	public Transform groundCheck, leftCheck, rightCheck, topCheck;

	// Global Variables
	private float groundCheckRadius = 0.3f;
	private bool _isGrounded, _hasJumped, _hasMorphed, _isAbleToMove, _isAbleToJump;
	private string _name;
	private string _sound;
	private float _speed;
	private float _jumpForce;
	private float _movementDirection;

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
		stateMachine.SetState(new IdleState(stateMachine, this));
	}

	protected virtual void Start()
	{
		Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
	}

	protected virtual void Update()
	{
		Debug.Log(stateMachine.GetCurrentState());
		stateMachine.UpdateState();
		FlipSprite();
		UpdateAirbornState();
	}

	protected virtual void FixedUpdate()
	{
		stateMachine.FixedUpdateState();
		_isGrounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		Walk();
	}

	protected virtual void LateUpdate()
	{
		stateMachine.LateUpdateState();
	}

	// ============================================================
	// Properties
	// ============================================================

	public string Name { get { return _name; } protected set { _name = value; } }

	public string Sound { get { return _sound; } protected set { _sound = value; } }

	public float Speed { get { return _speed; } set { _speed = value; } }

	public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

	public bool HasJumped { get { return _hasJumped; } protected set { _hasJumped = value; } }

	public bool IsGrounded { get { return _isGrounded; } protected set { _isGrounded = value; } }

	public bool HasMorphed { get { return _hasMorphed; } set { _hasMorphed = value; } }

	public bool IsAbleToMove { get { return _isAbleToMove; } set { _isAbleToMove = value; } }

	public bool IsAbleToJump { get { return _isAbleToJump; } set { _isAbleToJump = value; } }

	// Set Movement Direction as a property so we only enter the
	// walking & idle states once, and not every frame. 
	public float MovementDirection 
	{ 
		get { return _movementDirection; }
		set
		{
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
	// Ped Basic Tasks & States
	// ============================================================

	public void Jump()
	{
		if(_isAbleToJump)
		{
			_hasJumped = true;
			Animator.SetTrigger("takeOff");
			Rigidbody2D.velocity = Vector2.up * _jumpForce;
			_hasJumped = false;
		}
	}

	public void Walk()
	{
		if(_isAbleToMove)
		{
			Rigidbody2D.velocity = new Vector2(_movementDirection * _speed, Rigidbody2D.velocity.y);
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

	private void UpdateAirbornState()
	{
		if(_isGrounded)
		{
			Animator.SetBool("isAirborn", false);
		}
		else
		{
			Animator.SetBool("isAirborn", true);
		}
	}
}