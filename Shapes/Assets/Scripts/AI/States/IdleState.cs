﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	public IdleState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("isWalking", false);
	}

	public override void UpdateState()
	{
		if(ped.HasHitBallState)
		{
			ped.IsDead = true;
			ped.Rigidbody2D.AddForce(ped.transform.up * 180f);
			ped.Die();
		}
	}

	public override void ExitState()
	{

	}
}
