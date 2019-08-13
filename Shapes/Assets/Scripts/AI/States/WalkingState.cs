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
		
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		ped.Animator.SetBool("isWalking", false);
	}
}
