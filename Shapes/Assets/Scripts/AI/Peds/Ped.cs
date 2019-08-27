/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is the core script for peds such as the Player, enemies, allies etc.
* All peds inherit this script, where they can then access it's components,
* methods, properties and their State Machine.  
* Ai class is also here so everything's in 1 place. 
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

	public enum PedType
	{
		Player,
		Enemy
	}

	public enum Direction
	{
		Left = -1,
		Idle = 0,
		Right = 1
	}

	public enum States
	{
		Dead,
		Walk,
		Idle,
		Ball,
		Block,
		HorizontalShield,
		VerticalShield
	};

	// Classes
	protected StateMachine stateMachine;

	// Components
	[HideInInspector]
	public Rigidbody2D Rigidbody2D;
	[HideInInspector]
	public Collider2D Collider2D;
	[HideInInspector]
	public Animator Animator;
	[HideInInspector]
	public Animation Animation;

	[Header("Base Ped Components")]
	// GameObjects / Transforms
	[HideInInspector]
	public GameObject player;
	public LayerMask whatIsGround;
	public Transform areaColliders;
	public Transform groundCheck, leftCheck, rightCheck, topCheck;

	// Global Variables
	public PedType pedType { get; set; }
	public string Name { get; protected set; }
	public float Speed { get; set; }
	public float JumpForce { get; set; }
	public float GroundCheckRadius { get; set; }
	public float SideCheckRadius { get; set; }
	[SerializeField]
	private Quaternion rotation;
	private string _sound;
	private float _movementDirection;

	// Detect collisions around the ped to prevent morphing in tight spaces.
	public bool CollidedLeft { get { return Physics2D.OverlapCircle (leftCheck.position, SideCheckRadius, whatIsGround); } }
	public bool CollidedRight { get { return Physics2D.OverlapCircle (rightCheck.position, SideCheckRadius, whatIsGround); } }
	public bool CollidedTop { get { return Physics2D.OverlapCircle (topCheck.position, GroundCheckRadius * 3, whatIsGround); } }
	public bool IsGrounded { get { return Physics2D.OverlapCircle (groundCheck.position, GroundCheckRadius, whatIsGround); } }

	public float DistanceBetweenPedAndPlayer 
	{ 
		get 
		{ 
			if(player != null)
			return Vector2.Distance(player.transform.position, gameObject.transform.position);
			else
			return 999; 
		} 
	}

	public bool BlockAI { get; set; }

	public bool HasMorphed { get; set; }
	public bool HasJumped { get; protected set; }
	public bool IsAbleToMove { get; set; }
	public bool IsAbleToJump { get; set; }
	public bool IsDead { get; set; }
	public bool IsAlerted { get; set; }

	public bool HasHitGround { get; set; }
	public bool HasHitWater { get; set; }
	public bool HasHitPlayer { get; set; }
	public bool HasHitEnemy { get; set; }
	public bool HasHitBlockState { get; set; }
	public bool HasHitBallState { get; set; }
	public bool HasHitHorizontalShieldState { get; set; }
	public bool HasHitVerticalShieldState { get; set; }

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
		player = GameObject.FindGameObjectWithTag("Player");
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
		//Debug.Log(Name + ": " + stateMachine.GetCurrentState());
		FaceBodyInCorrectDirection();
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

	// Set Movement Direction as a property so the state is
	// automatically applied when moving. 
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
				if(_movementDirection != (float)Direction.Idle)
				{
					SetPedState(States.Walk);
				}
				else
				{
					SetPedState(States.Idle);
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

	private void FaceBodyInCorrectDirection()
	{
		if(!HasMorphed)
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

	// Any ped can call this method to set a morph state, and not have to worry about
	// adding additional functionality to only call the state for one frame. 
	public void SetPedState(States state)
	{
		if(HasMorphed && !IsDead)
		{
			return;
		}
		else
		{
			switch(state)
			{
				case States.Dead:
				stateMachine.SetState(new DeadState(stateMachine, this));
				break;
				case States.Walk:
				stateMachine.SetState(new WalkingState(stateMachine, this));
				break;
				case States.Idle:
				stateMachine.SetState(new IdleState(stateMachine, this));
				break;
				case States.Ball: 
				stateMachine.SetState(new MorphIntoBallState(stateMachine, this));
				break;
				case States.Block:
				stateMachine.SetState(new MorphIntoBlockState(stateMachine, this));
				break;
				case States.HorizontalShield:
				stateMachine.SetState(new MorphIntoHorizontalShieldState(stateMachine, this));
				break;
				case States.VerticalShield:
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
		HandleCollisions(col, true);
	}

	private void OnCollisionExit2D(Collision2D col)
	{
		for(int i = 0; i < 2; i++)
		{
			HandleCollisions(col, false);
		}
		//HandleCollisions(col, false);
	}

	public void HandleCollisions(Collision2D col, bool boolValue)
	{
		if(col.gameObject.tag == "Player")
		{
			HasHitPlayer = boolValue;
		}
		else if(col.gameObject.tag == "Enemy")
		{
			HasHitEnemy = boolValue;
		}

		if(col.gameObject.layer == LayerMask.NameToLayer("Ground"))
		{
			HasHitGround = boolValue;
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("Water"))
		{
			HasHitWater = boolValue;
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("Ball"))
		{
			HasHitBallState = boolValue;
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("Block"))
		{
			HasHitBlockState = boolValue;
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("VerticalShield"))
		{
			HasHitVerticalShieldState = boolValue;
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("HorizontalShield"))
		{
			HasHitHorizontalShieldState = boolValue;
		}
	}

	public void ChangeLayerMask(States stateLayer)
	{
		string layer = stateLayer.ToString();
		this.gameObject.layer = LayerMask.NameToLayer(layer);
		foreach(Transform child in this.gameObject.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer(layer);
		}
	}

	public void RevertLayerMask()
	{
		string ped = this.pedType.ToString();
		this.gameObject.layer = LayerMask.NameToLayer(ped);
		foreach(Transform child in this.gameObject.transform)
		{
			child.gameObject.layer = LayerMask.NameToLayer(ped);
		}
	}

	// ============================================================
	// What happens when the ped dies? :/ 
	// ============================================================

	public void Die()
	{
		IsDead = true;
		SetPedState(States.Dead);
	}

	public void Destroy()
	{
		Destroy(this.gameObject);
	}
}