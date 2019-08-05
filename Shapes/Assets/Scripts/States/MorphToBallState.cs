/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when the player has
* morphed into a ball.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphToBallState : State
{
	private float ballSpeed = 2f;
	private Vector2 movement;

	public MorphToBallState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}
	

	public override void EnterState()
	{
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Speed += ballSpeed;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBall", true);
	}

	public override void UpdateState()
	{
		
	}

	public override void FixedUpdateState()
	{
		AddForce();
	}

	public override void ExitState()
	{
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.Speed -= ballSpeed;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToBall", false);
	}

	// Give the player some sort of control whist morphed into a ball. 
	private void AddForce()
	{
		movement = new Vector2 (ped.MovementDirection, 0);
		ped.Rigidbody2D.AddForce(movement * ballSpeed);
	}
}
