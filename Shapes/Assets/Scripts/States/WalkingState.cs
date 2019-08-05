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
		Debug.Log("ENTER WALKING");
	}

	public override void UpdateState()
	{
		
	}

	public override void FixedUpdateState()
	{
		base.FixedUpdateState();
		ped.Animator.SetBool("isWalking", true);
	}

	public override void ExitState()
	{
		Debug.Log("EXIT WALKING");
	}
}
