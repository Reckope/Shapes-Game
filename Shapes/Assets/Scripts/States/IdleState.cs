﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : GroundedState
{
	public IdleState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		base.EnterState();
	}

	public override void UpdateState()
	{
		base.UpdateState();
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
		ped.Animator.SetBool("isWalking", false);
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}
