using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State
{
	public WalkingState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{
		
	}

	public override void EnterState()
	{
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

		if(ped.HasHitBallState || ped.HasHitBlockState)
		{
			ped.IsDead = true;
			ped.Rigidbody2D.AddForce(ped.transform.up * 180f);
			ped.Die();
		}
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		ped.Animator.speed = 1;
		ped.Animator.SetBool("isWalking", false);
	}
}
