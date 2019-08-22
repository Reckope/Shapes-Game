/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the player.
* Attach this to the player GameObject,
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ped
{
	// ============================================================
	// Player Variables
	// ============================================================

	[Header("Player Settings")]
	[SerializeField]
	private string _name = "Morphy";
	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, _groundCheckRadius = 0.2f, blockCheckRadius = 3.4f;

	[Header("Player Components & GameObjects")]
	public Transform morphIntoBlockCheck;
	public GameObject blockFeedback;
	public bool CantMorphIntoBlock { get { return Physics2D.OverlapCircle (morphIntoBlockCheck.position, blockCheckRadius, whatIsGround); } }

	// ============================================================
	// MonoBehaviour methods
	// ============================================================

	// We override all of the peds' MonoBehaviour methods to 
	// inherit any components, methods etc, whilst adding extra code here. 
	protected override void Awake()
	{
		base.Awake();
		pedType = PedType.Player;
		Name = _name;
		Speed = _speed;
		JumpForce = _jumpForce;
		GroundCheckRadius = _groundCheckRadius;
		blockFeedback.SetActive(false);
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.SetState(new IdleState(stateMachine, this));
	}

	protected override void Update()
	{
		base.Update();
		HandlePlayerInput();
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
				HasJumped = true;
			}
		}
		else
		{
			if(MorphToBlockInput && !CantMorphIntoBlock)
			{
				SetPedState(States.Block);
			}
		}

		if(MorphToBallInput)
		{
			SetPedState(States.Ball);
		}

		if(MorphToBlockFeedback && CantMorphIntoBlock && !HasMorphed)
		{
			StartCoroutine(UnableToMorphToBlock());
		}
	}

	private IEnumerator UnableToMorphToBlock()
	{
		//this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 180, 180, 255);
		blockFeedback.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		blockFeedback.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		blockFeedback.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		blockFeedback.SetActive(false);
		//this.gameObject.GetComponentInChildren<SpriteRenderer>().color = new Color(255, 255, 255, 255);
	}
}
