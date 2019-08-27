using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State
{
	private float flinchUpwardsForce = 280f;
	private float bounceBackForce = 120f;

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
			ped.Animator.speed = ped.Speed - (ped.Speed / 11f);
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
		ped.HasHitBallState += ped.Die;
		ped.HasHitBlockState += ped.Destroy;
		ped.HasHitHorizontalShieldState += TakeOff;
		ped.HasHitVerticalShieldState += BounceBack;
	}

	private void UnsubscribeToPedInteractionEvents()
	{
		ped.HasHitBallState -= ped.Die;
		ped.HasHitBlockState -= ped.Destroy;
		ped.HasHitHorizontalShieldState -= TakeOff;
		ped.HasHitVerticalShieldState -= BounceBack;
	}

	private void TakeOff()
	{
		if(!ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}
		else
		{
			BounceBack();
		}
	}

	private void BounceBack()
	{
		ped.StartCoroutine(Bounce());
	}

	private IEnumerator Bounce()
	{
		ped.IsAbleToMove = false;
		if(ped.MovementDirection == (int)Ped.Direction.Left)
		{
			ped.Rigidbody2D.AddForce(Vector2.right * bounceBackForce);
		}
		else if(ped.MovementDirection == (int)Ped.Direction.Right)
		{
			ped.Rigidbody2D.AddForce(Vector2.left * bounceBackForce);
		}
		yield return new WaitForSeconds(0.5f);
		ped.IsAbleToMove = true;
	}
}
