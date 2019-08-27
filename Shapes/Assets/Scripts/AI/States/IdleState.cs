using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	public IdleState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("isWalking", false);
	}

	public override void UpdateState()
	{
		/*if(ped.HasHitBallState)
		{
			ped.Rigidbody2D.AddForce(ped.transform.up * 180f);
			ped.Die();
		}
		else if(ped.HasHitBlockState)
		{
			ped.Die();
		}
		else if(ped.HasHitHorizontalShieldState && !ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}*/
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
		ped.HasHitBallState += ped.Die;
		ped.HasHitHorizontalShieldState += HitByHorizontalShield;
	}

	private void UnsubscribeFromInteractionEvents()
	{
		ped.HasHitBlockState -= ped.Destroy;
		ped.HasHitBallState -= ped.Die;
		ped.HasHitHorizontalShieldState -= HitByHorizontalShield;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitByBall()
	{
		ped.Rigidbody2D.AddForce(ped.transform.up * 180f);
		ped.Die();
	}

	private void HitByBlock()
	{
		// Sound
		ped.Die();
	}

	private void HitByHorizontalShield()
	{
		if(!ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}
	}
}
