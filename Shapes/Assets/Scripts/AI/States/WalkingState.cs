using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State
{
	private float walkingAnimationSpeed = 11f;

	public WalkingState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{
		
	}

	public override void EnterState()
	{
		SubscribeToPedInteractionEvents();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.Animator.SetBool("isWalking", true);
	}

	public override void UpdateState()
	{
		if(ped.IsGrounded)
		{
			ped.Animator.speed = ped.Speed - (ped.Speed / walkingAnimationSpeed);
		}
		else
		{
			ped.Animator.speed = 1;
		}
	}

	public override void FixedUpdateState()
	{

	}

	public override void ExitState()
	{
		UnsubscribeToPedInteractionEvents();
		ped.Animator.speed = 1;
		ped.Animator.SetBool("isWalking", false);
	}

	// ==============================================================
	// Events - What happens when an event triggers during this state? 
	// ==============================================================

	private void SubscribeToPedInteractionEvents()
	{
		ped.HasHitBallState += ped.TakeDamage;
		ped.HasHitBlockState += ped.Destroy;
		ped.HasHitHorizontalShieldState += TakeOff;
		ped.HasHitVerticalShieldState += TakeOff;
	}

	private void UnsubscribeToPedInteractionEvents()
	{
		ped.HasHitBallState -= ped.TakeDamage;
		ped.HasHitBlockState -= ped.Destroy;
		ped.HasHitHorizontalShieldState -= TakeOff;
		ped.HasHitVerticalShieldState -= TakeOff;
	}

	private void TakeOff()
	{
		if(!ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}
		else
		{
			ped.BounceAway();
		}
	}
}
