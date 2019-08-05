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
	private bool grounded, jumped, _morphed;
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
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		Walk();
	}

	// ============================================================
	// Get Set methods / properties.
	// ============================================================

	public string Name { get { return _name; } set { _name = value; } }

	public string Sound { get { return _sound; } set { _sound = value; } }

	public float Speed { get { return _speed; } set { _speed = value; } }

	public float JumpForce { get { return _jumpForce; } set { _jumpForce = value; } }

	public bool Morphed { get { return _morphed; } set { _morphed = value; } }

	// Set Movement Direction as a property so we only enter the
	// walking & idle states once, and not every frame. 
	public float MovementDirection 
	{ 
		get 
		{
			return _movementDirection; 
		}
		set 
		{
			if(value == _movementDirection)
			{
				return;
			}
			if(!Morphed)
			{
				stateMachine.SetState(new WalkingState(stateMachine, this));
			}
			_movementDirection = value;
			if(_movementDirection == 0 && !Morphed)
			{
				stateMachine.SetState(new IdleState(stateMachine, this));
			}
		}
	}

	// ============================================================
	// Ped Tasks
	// ============================================================

	public void Jump()
	{
		jumped = true;
		Animator.SetTrigger("takeOff");
		Rigidbody2D.velocity = Vector2.up * _jumpForce;
		jumped = false;
	}

	public void Walk()
	{
		Rigidbody2D.velocity = new Vector2(_movementDirection * _speed, Rigidbody2D.velocity.y);
	}

	private void FlipSprite()
	{
		if(Rigidbody2D.velocity.x > 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		else if(Rigidbody2D.velocity.x < 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
	}

	private void UpdateAirbornState()
	{
		if(grounded)
		{
			Animator.SetBool("isAirborn", false);
		}
		else
		{
			Animator.SetBool("isAirborn", true);
		}
	}

	// ============================================================
	// Ped Actions
	// ============================================================

	public bool IsGrounded() { return grounded; }

	public bool HasJumped() { return jumped; }

	public bool HasMorphed() { return _morphed; }
}