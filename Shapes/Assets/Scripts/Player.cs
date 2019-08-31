/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the player.
* Attach this to the player GameObject,
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ped
{
	// There can only ever be one instance of a player.
	public static Player Instance { get { return _instance; } }
	private static Player _instance;

	// ============================================================
	// Player Variables
	// ============================================================

	[Header("Player Settings")]
	[SerializeField]
	private string _name = "Morphy";
	[SerializeField][Range(0.1f, 15.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, blockCheckRadius = 1.4f;
	private bool isDead;

	[SerializeField][Range(0.1f, 0.5f)]
	private float _sideCheckRadius = 0.1f, _groundCheckRadius = 0.2f;

	[Header("Player Components & GameObjects")]
	public Transform morphIntoBlockCheck;
	public GameObject blockFeedback;
	public bool CanMorphIntoBlock 
	{ 
		get { return !Physics2D.OverlapCircle (morphIntoBlockCheck.position, blockCheckRadius, whatIsGround); } 
	}

	public int Lives { get; set; }
	public event Action<int> OnLivesChanged;

	// ============================================================
	// MonoBehaviour methods
	// ============================================================

	// We override all of the peds' MonoBehaviour methods to 
	// inherit any components, methods etc, whilst adding extra code here. 
	protected override void Awake()
	{
		base.Awake();

		if(_instance != null && _instance != this)
		{
			Debug.LogError("Error: Another instance of Player has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}

		pedType = PedType.Player;
		Name = _name;
		GroundCheckRadius = _groundCheckRadius;
		SideCheckRadius = _sideCheckRadius;
		blockFeedback.SetActive(false);
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.SetState(new IdleState(stateMachine, this));
		Lives = 3;
	}

	protected override void Update()
	{
		base.Update();

		Debug.Log(_name + "'s Lives: " + Lives);
		isDead = IsDead;
		Speed = _speed;
		JumpForce = _jumpForce;
		if(!isDead)
		{
			HandlePlayerInput();
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
	}

	// ============================================================
	// Other Player Methods
	// ============================================================

	private void HandlePlayerInput()
	{
		bool MorphToBlockFeedback = Input.GetKeyDown("up");

		MovementDirection = Input.GetAxisRaw("Horizontal");
		float jump = Input.GetAxisRaw("Jump");

		// Returns true every frame. This is used to detect when the 
		// player has released the key associated with the state.
		MorphToBallInput = Input.GetKey("down");
		MorphToBlockInput = Input.GetKey("up");
		MorphToHorizontalShieldInput = Input.GetKey("right");
		MorphToVerticalShieldInput = Input.GetKey("left");

		// The player can enter the following morph states.
		if(IsGrounded)
		{
			if(MorphToHorizontalShieldInput)
			{
				SetPedState(States.HorizontalShield);
			}
			if(MorphToVerticalShieldInput)
			{
				SetPedState(States.VerticalShield);
			}
			if(jump > 0)
			{
				Jumped = true;
			}
		}
		else
		{
			if(MorphToBlockInput && DistanceFromGround() > 3f && CanMorphIntoBlock)
			{
				blockFeedback.SetActive(false);
				SetPedState(States.Block);
			}
		}

		if(MorphToBallInput)
		{
			SetPedState(States.Ball);
		}

		if(MorphToBlockFeedback && !CanMorphIntoBlock && !HasMorphed)
		{
			blockFeedback.SetActive(true);
		}

		if(!MorphToBlockInput)
		{
			blockFeedback.SetActive(false);
		}
	}

	public void LoseLives(int life)
	{
		if (Lives - life > 0) 
		{
			Lives -= life;
			if (OnLivesChanged != null)
			{
				Debug.Log("Lost life3");
				OnLivesChanged(Lives);
			}
		}
		else
		{
			Lives = 0;
			IsDead = true;
			stateMachine.SetState(new DeadState(stateMachine, this));
		}
	}

	public void IncreaseLives(int life)
	{
		if (Lives + life > 0) 
		{
			Lives += life;
			if (OnLivesChanged != null)
			{
				OnLivesChanged(Lives);
			}
		}
	}
}
