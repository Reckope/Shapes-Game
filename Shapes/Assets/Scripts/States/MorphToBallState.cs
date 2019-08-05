using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphToBallState : State
{
	public MorphToBallState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		ped.Morphed = true;
		ped.Animator.SetBool("morphToBall", true);
	}

	public override void UpdateState()
	{
		
	}

	public override void FixedUpdateState()
	{
		ped.Walk();
	}

	public override void ExitState()
	{
		ped.Morphed = false;
		ped.Animator.SetBool("morphToBall", false);
	}
}
