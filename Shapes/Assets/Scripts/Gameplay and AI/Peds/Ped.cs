﻿/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is the core script for peds such as the Player, enemies, allies etc.
* All peds inherit this script, where they can then access it's components,
* methods, properties and the State Machine.  
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
	// Ped Enums
	// ============================================================

	public enum PedType
	{
		Player,
		Enemy
	}

	public enum PedNames
	{
		Morphy,
		Dynamo,
		Cinder,
		Aegis,
		Priwen
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

	public enum HarmfulObjects
	{
		Saw
	}

	public enum DamageType
	{
		Destroy,
		Hit
	}
	DamageType damageType;

	// ============================================================
	// Everything a healthy ped needs.
	// ============================================================

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

	// GameObjects / Transforms
	[HideInInspector]
	public GameObject player;
	[Header("Base Ped Components")]
	public LayerMask whatIsGround;
	public Transform areaColliders;
	public Transform groundCheck, leftCheck, rightCheck, topCheck;

	// Global Variables
	public PedType pedType { get; set; }
	public string Name { get; protected set; }
	public float Speed { get; set; }
	public float JumpForce { get; set; }
	public int FaceDirection { get; set; }
	public float GroundCheckRadius { get; set; }
	public float SideCheckRadius { get; set; }
	private Quaternion rotation;
	private string _sound;
	private float _movementDirection = 0.5f;

	// Detect collisions around the ped to prevent morphing in tight spaces.
	public bool CollidedLeft { get { return Physics2D.OverlapCircle (leftCheck.position, SideCheckRadius, whatIsGround); } }
	public bool CollidedRight { get { return Physics2D.OverlapCircle (rightCheck.position, SideCheckRadius, whatIsGround); } }
	public bool CollidedTop { get { return Physics2D.OverlapCircle (topCheck.position, GroundCheckRadius * 3, whatIsGround); } }
	public bool IsGrounded { get { return Physics2D.OverlapCircle (groundCheck.position, GroundCheckRadius, whatIsGround); } }

	public bool EnemyCollidedLeft { get; set; }
	public bool EnemyCollidedRight { get; set; }
	public bool EnemyCollidedTop { get; set; }

	public bool HasMorphed { get; set; }
	public bool Jumped { get; protected set; }
	public bool IsAbleToMove { get; set; }
	public bool IsAbleToJump { get; set; }
	public bool IsDead { get; set; }
	public bool IsAlerted { get; set; }

	// Interaction Events
	public event Action HasHitGround;
	public event Action HasHitWater;
	public event Action HasHitPlayer;
	public event Action HasHitEnemy;
	public event Action HasHitBallState;
	public event Action HasHitBlockState;
	public event Action HasHitHorizontalShieldState;
	public event Action HasHitVerticalShieldState;
	public event Action HasHitSaw;

	// Player Input
	public bool MorphToBallInput { get; protected set; }
	public bool MorphToBlockInput { get; protected set; }
	public bool MorphToHorizontalShieldInput { get; protected set; }
	public bool MorphToVerticalShieldInput { get; protected set; }

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
		HasHitSaw += HitSaw;
		rotation = areaColliders.transform.rotation;
		IsAbleToMove = true;
		IsAbleToJump = true;
	}

	protected virtual void Update()
	{
		stateMachine.UpdateState();

		//Debug.Log(Name + ": " + stateMachine.CurrentState);
		DistanceFromGround();
		UpdateAirbornAnim();
		FaceBodyInCorrectDirection();
		areaColliders.transform.rotation = rotation;
	}

	protected virtual void FixedUpdate()
	{
		stateMachine.FixedUpdateState();

		Walk();
		if(Jumped)
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
				if(_movementDirection != (int)Direction.Idle)
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

	public float DistanceBetweenPedAndPlayer 
	{ 
		get 
		{ 
			if(player != null)
			{
				return Vector2.Distance(player.transform.position, gameObject.transform.position);
			}
			else
			{
				return 999; 
			}
		} 
	}

	// ============================================================
	// Ped Basic Tasks, States & Movement.
	// ============================================================

	public void Jump()
	{
		if(IsAbleToJump)
		{
			Animator.SetTrigger("takeOff");
			Rigidbody2D.velocity = Vector2.up * JumpForce;
			Jumped = false;
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
				FaceDirection = 1;
			}
			else if(Rigidbody2D.velocity.x < 0 || FaceDirection == -1)
			{
				FaceDirection = -1;
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
		Collider2D collider = col.collider;

		Vector3 contactPoint = col.contacts[0].point;
		Vector3 center = collider.bounds.center;

		float centerOffset = 0.06f;
		
		// Detect where the enemy or player collided with each other.
		if(col.gameObject.layer == LayerMask.NameToLayer(PedType.Enemy.ToString()) 
		|| col.gameObject.layer == LayerMask.NameToLayer(PedType.Player.ToString()))
		{
			EnemyCollidedTop = contactPoint.y < center.y - centerOffset;
			EnemyCollidedLeft = contactPoint.x > center.x;
			EnemyCollidedRight = contactPoint.x < center.x;
		}
		HandleCollisions(col, true);
	}

	public void HandleCollisions(Collision2D col, bool boolValue)
	{
		if(col.gameObject.CompareTag(States.Ball.ToString()) && HasHitBallState != null)
		{
			HasHitBallState();
		}
		else if(col.gameObject.CompareTag(States.Block.ToString()) && HasHitBlockState != null)
		{
			HasHitBlockState();
		}
		else if(col.gameObject.CompareTag(States.VerticalShield.ToString()) && HasHitVerticalShieldState != null)
		{
			HasHitVerticalShieldState();
		}
		else if(col.gameObject.CompareTag(States.HorizontalShield.ToString()) && HasHitHorizontalShieldState != null)
		{
			HasHitHorizontalShieldState();
		}
		else if(col.gameObject.CompareTag(HarmfulObjects.Saw.ToString()) && HasHitSaw!= null)
		{
			HasHitSaw();
		}

		if(col.gameObject.layer == LayerMask.NameToLayer(PedType.Enemy.ToString()) && HasHitEnemy != null)
		{
			HasHitEnemy();
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer(PedType.Player.ToString()) && HasHitPlayer != null)
		{
			HasHitPlayer();
		}

		if(col.gameObject.layer == LayerMask.NameToLayer("Ground") && HasHitGround != null)
		{
			HasHitGround();
		}
		else if(col.gameObject.layer == LayerMask.NameToLayer("Water") && HasHitWater != null)
		{
			HasHitWater();
		}
	}

	public void ChangeTag(States stateTag)
	{
		string tag = stateTag.ToString();

		this.gameObject.tag = tag;
		foreach(Transform child in this.gameObject.transform)
		{
			child.gameObject.tag = tag;
		}
	}

	public void RevertTag()
	{
		string ped = this.pedType.ToString();

		this.gameObject.tag = ped;
		foreach(Transform child in this.gameObject.transform)
		{
			this.gameObject.tag = ped;
		}
	}

	// ============================================================
	// What happens when the ped dies? :/ 
	// ============================================================

	// Use an enum to determine death - Destroy or hit
	public void TakeDamage(DamageType DamageType)
	{
		if(DamageType == DamageType.Hit)
		{
			if(pedType == PedType.Player)
			{
				if(!Player.Instance.isInvulnerable)
				{
					Player.Instance.LoseLives(1);
				}
			}
			else
			{
				if(Name != PedNames.Cinder.ToString())
				{
					GameManager.Instance.StartCoroutine(GameManager.Instance.EnableActionShot());
				}
				Die();
			}
		}
		else if(DamageType == DamageType.Destroy)
		{
			HandleDeadPedData();
			if(pedType == PedType.Player)
			{
				Player.Instance.LoseLives(Player.Instance.Lives);
				transform.localScale = new Vector3(0f, 0f, 0f);
			}
			else
			{
				GameManager.Instance.StartCoroutine(GameManager.Instance.EnableActionShot());
				Destroy();
			}
		}
	}

	// Data will increment no matter how the ped dies.
	// This is so if the player lures the enemy into some harmful environmental
	// object, the payer will still get the point for kiling an enemy.
	protected void Die()
	{
		IsDead = true;
		HandleDeadPedData();
		stateMachine.SetState(new DeadState(stateMachine, this));
	}

	public void Destroy()
	{
		IsDead = true;
		//Destroy(this.gameObject);
		this.gameObject.SetActive(false);
	}

	private void HitSaw()
	{
		// Sound
		TakeDamage(DamageType.Destroy);
	}

	private void HandleDeadPedData()
	{
		if(Name == PedNames.Dynamo.ToString())
		{
			GameData.IncrementPlayerStatsData(GameData.PlayerStatIDs.DynamoKilled);
		}
		else if(Name == PedNames.Cinder.ToString())
		{
			GameData.IncrementPlayerStatsData(GameData.PlayerStatIDs.CinderKilled);
		}
		else if(Name == PedNames.Aegis.ToString())
		{
			Debug.Log("Aegis Killed");
			GameData.IncrementPlayerStatsData(GameData.PlayerStatIDs.AegisKilled);
		}
		else if(Name == PedNames.Priwen.ToString())
		{
			GameData.IncrementPlayerStatsData(GameData.PlayerStatIDs.PriwenKilled);
		}
		else if(pedType == PedType.Player)
		{
			GameData.IncrementPlayerStatsData(GameData.PlayerStatIDs.TotalDeaths);
		}
	}

	// ============================================================
	// Useful ped info and actions other class can access
	// ============================================================

	public float DistanceFromGround()
	{
		Vector2 visionSpawn;

		if(groundCheck != null)
		{
			visionSpawn = groundCheck.transform.position;
		}
		else
		{
			Debug.LogError("GroundCheck is missing. Can't determine distance from ground");
			return 0;
		}

		Vector2 visionDirection = Vector2.down;
		float visionDistance = 9999f;

		RaycastHit2D distanceFromGround = Physics2D.Raycast(visionSpawn, visionDirection, visionDistance);

		return distanceFromGround.distance;
	}

	// Bounce away from objects. 
	public void BounceAway()
	{
		StartCoroutine(Bounce());
	}

	private IEnumerator Bounce()
	{
		Vector2 bounceDirection = new Vector2(-(int)MovementDirection, 0);
		float bounceAwayForce = 120f;

		IsAbleToMove = false;
		Rigidbody2D.AddForce(bounceDirection * bounceAwayForce);
		yield return new WaitForSeconds(0.5f);
		IsAbleToMove = true;
	}
}