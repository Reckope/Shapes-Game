/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when the player has
* morphed into a ball.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBallState : State
{
	private float ballSpeed = 4f;
	private bool isAbleToAddForce;

	public MorphIntoBallState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.ChangeTag(Ped.States.Ball);
		SubscribeToInteractionEvents();
		isAbleToAddForce = true;
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Speed += ballSpeed;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBall", true);
	}

	public override void UpdateState()
	{
		/*if(ped.HasHitVerticalShieldState || ped.HasHitBlockState || (ped.HasHitHorizontalShieldState && !ped.IsGrounded))
		{
			isAbleToAddForce = false;
			ped.Die();
		}*/
		
		if(!ped.IsDead)
		{
			if(ped.pedType == Ped.PedType.Player)
			{
				PlayerControls();
			}
			else
			{
				MoveTowardsPlayer();
			}
		}
	}

	public override void FixedUpdateState()
	{
		if(ped.IsGrounded)
		{
			AddForce();
		}
	}

	public override void ExitState()
	{
		if(!ped.IsDead)
		{
			ped.RevertTag();
		}
		UnsubscribeToInteractionEvents();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.Speed -= ballSpeed;
		if(!ped.IsDead)
		{
			ped.transform.rotation = Quaternion.identity;
			ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		}

		ped.Animator.SetBool("morphToBall", false);
	}

	// ==============================================================
	// Events - What happens when an event triggers during this state? 
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += Die;
		//ped.HasHitBallState += Die;
		ped.HasHitHorizontalShieldState += HitByHorizontalShield;
		ped.HasHitVerticalShieldState += Die;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitBlockState -= Die;
		//ped.HasHitBallState -= Die;
		ped.HasHitHorizontalShieldState -= HitByHorizontalShield;
		ped.HasHitVerticalShieldState -= Die;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitByBall()
	{
		
	}

	private void HitByBlock()
	{
		
	}

	private void HitByHorizontalShield()
	{
		Debug.Log("Ouch");
	}

	private void HitByVerticalShield()
	{

	}

	private void Die()
	{
		//ped.HasHitVerticalShieldState -= Die;
		isAbleToAddForce = false;
		ped.Die();
	}

	// ============================================================
	// Private Methods for when the ped has morphed into a ball.
	// ============================================================

	// Give the player / ped some sort of control, but not as much as walking normally.
	private void AddForce()
	{
		if(isAbleToAddForce && !ped.IsDead)
		{
			Vector2 movement = new Vector2 (ped.MovementDirection, 0);
			ped.Rigidbody2D.AddForce(movement * ballSpeed);
		}
	}

	private void PlayerControls()
	{
		if(!ped.MorphToBallInput && !ped.CollidedTop && (!ped.CollidedLeft || !ped.CollidedRight))
		{
			ped.ExitMorphState();
		}
	}

	private void MoveTowardsPlayer()
	{
		if(ped.player.transform.position.x < ped.transform.position.x)
		{
			ped.MovementDirection = -1;
		}
		else if(ped.player.transform.position.x > ped.transform.position.x)
		{
			ped.MovementDirection = 1;
		}
	}
}
