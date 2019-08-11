/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the player.
* Attach this to the player GameObject,
* WORK IN PROGRESS
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ped
{
	// Global Variables
	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, _upwardForce = 180f, _groundCheckRadius = 0.1f;

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
		IsAbleToMove = true;
		IsAbleToJump = true;
	}

	protected override void Update()
	{
		base.Update();
		HandleInput();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	// ============================================================
	// Handle all the input for the player. 
	// ============================================================

	private void HandleInput()
	{
		MovementDirection = Input.GetAxisRaw("Horizontal");

		if(Input.GetKeyDown("down"))
		{
			stateMachine.SetState(new MorphToBallState(stateMachine, this));
		}

		if(Input.GetKeyDown("up") && !CantMorphIntoBlock)
		{
			Rigidbody2D.AddForce(transform.up * _upwardForce);
			stateMachine.SetState(new MorphToBlockState(stateMachine, this));
		}

		if(Input.GetKeyDown("right") && IsGrounded)
		{
			stateMachine.SetState(new MorphToHorizontalShieldState(stateMachine, this));
		}

		if(Input.GetKeyDown("left") && IsGrounded)
		{
			stateMachine.SetState(new MorphToVerticalShieldState(stateMachine, this));
		}

		if(Input.GetKeyDown(KeyCode.W) && IsGrounded)
		{
			HasJumped = true;
		}
	}
}
