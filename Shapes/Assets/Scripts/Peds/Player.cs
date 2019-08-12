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

	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, _groundCheckRadius = 0.1f;

	// ============================================================
	// MonoBehaviour methods
	// ============================================================

	// We override all of the peds' MonoBehaviour methods to 
	// inherit any components, methods etc, whilst adding extra code here. 
	protected override void Awake()
	{
		base.Awake();
		Name = "Morphy";
		Speed = _speed;
		JumpForce = _jumpForce;
		GroundCheckRadius = _groundCheckRadius;
	}

	protected override void Start()
	{
		base.Start();
		stateMachine.SetState(new IdleState(stateMachine, this));
		IsAbleToJump = true;
		IsAbleToMove = true;
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

	// ============================================================
	// Other Player Methods
	// ============================================================

	private void HandlePlayerInput()
	{
		MovementDirection = Input.GetAxisRaw("Horizontal");
		float jump = Input.GetAxisRaw("Jump");

		// Returns true every frame. This is used to detect when the 
		// player has released the key associated with the state.
		MorphToBallInput = Input.GetKey("down");
		MorphToBlockInput = Input.GetKey("up");
		MorphToHorizontalShieldInput = Input.GetKey("right");
		MorphToVerticalShieldInput = Input.GetKey("left");

		// The player can enter the following states.
		if(IsGrounded)
		{
			if(MorphToHorizontalShieldInput)
			{
				SetMorphState(MorphStates.HorizontalShield);
			}
			if(MorphToVerticalShieldInput)
			{
				SetMorphState(MorphStates.VerticalShield);
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
				SetMorphState(MorphStates.Block);
			}
		}
		if(MorphToBallInput)
		{
			SetMorphState(MorphStates.Ball);
		}
	}
}
