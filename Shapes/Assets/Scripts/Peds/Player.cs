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
	private float playerSpeed = 0.1f, playerJumpForce = 0.1f;

	protected override void Awake()
	{
		base.Awake();
		Name = "Morphy";
		Speed = playerSpeed;
		JumpForce = playerJumpForce;
		IsAbleToMove = true;
		IsAbleToJump = true;
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		MovementDirection = Input.GetAxisRaw("Horizontal");
		Walk();

		if(Input.GetKeyDown("u"))
		{
			stateMachine.SetState(new MorphToBallState(stateMachine, this));
		}
		else if (Input.GetKeyUp("u"))
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

		if(Input.GetKeyDown(KeyCode.W) && IsGrounded)
		{
			Jump();
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}
