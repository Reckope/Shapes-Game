﻿/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds Idle.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	// Global Variables
	private float flinchUpwardsForce = 230f;

	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public IdleState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================

	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("isWalking", false);
	}

	public override void ExitState()
	{
		UnsubscribeFromInteractionEvents();
	}

	// ==============================================================
	// Events trigger certain methods exclusive to idling.
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += HitByBlock;
		ped.HasHitBallState += HitByBall;
		ped.HasHitHorizontalShieldState += HitByHorizontalShield;
	}

	private void UnsubscribeFromInteractionEvents()
	{
		ped.HasHitBlockState -= HitByBlock;
		ped.HasHitBallState -= HitByBall;
		ped.HasHitHorizontalShieldState -= HitByHorizontalShield;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitByBall()
	{
		ped.Rigidbody2D.AddForce(ped.transform.up * flinchUpwardsForce);
		ped.TakeDamage(Ped.DamageType.Hit);
	}

	private void HitByBlock()
	{
		// Sound
		ped.TakeDamage(Ped.DamageType.Destroy);
	}

	private void HitByHorizontalShield()
	{
		if(!ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}
	}
}
