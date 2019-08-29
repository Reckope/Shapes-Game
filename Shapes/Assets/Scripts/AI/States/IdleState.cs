﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	public IdleState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped){}

	private float flinchUpwardsForce = 180f;

	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("isWalking", false);
	}

	public override void ExitState()
	{
		UnsubscribeFromInteractionEvents();
	}

	// ==============================================================
	// Events - What happens when an event triggers during this state? 
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += ped.Destroy;
		ped.HasHitBallState += ped.TakeDamage;
		ped.HasHitHorizontalShieldState += HitByHorizontalShield;
	}

	private void UnsubscribeFromInteractionEvents()
	{
		ped.HasHitBlockState -= ped.Destroy;
		ped.HasHitBallState -= ped.TakeDamage;
		ped.HasHitHorizontalShieldState -= HitByHorizontalShield;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitByBall()
	{
		ped.Rigidbody2D.AddForce(ped.transform.up * flinchUpwardsForce);
		ped.TakeDamage();
	}

	private void HitByBlock()
	{
		// Sound
		ped.TakeDamage();
	}

	private void HitByHorizontalShield()
	{
		if(!ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}
	}
}
