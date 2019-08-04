using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : GroundedState
{
	public WalkingState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
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
		ped.Animator.SetBool("isWalking", true);
	}

	public override void ExitState()
	{
		base.ExitState();
	}
}
