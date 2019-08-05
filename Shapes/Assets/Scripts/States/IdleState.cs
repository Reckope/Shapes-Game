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
		ped.Animator.SetBool("isWalking", false);
	}

	public override void UpdateState()
	{
		
	}

	public override void FixedUpdateState()
	{
		//ped.Animator.SetBool("isWalking", false);
	}

	public override void ExitState()
	{
		
	}
}
