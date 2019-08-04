using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirbornState : State
{
	public AirbornState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		
	}

	public override void FixedUpdateState()
	{
		ped.Animator.SetBool("isAirborn", true);
		ped.Animator.SetBool("isWalking", false);
		ped.Walk();
	}

	public override void ExitState()
	{
		
	}
}
