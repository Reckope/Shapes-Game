using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedState : State
{
	public GroundedState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		
	}

	public override void UpdateState()
	{
		if(Input.GetKeyDown(KeyCode.W))
		{
			ped.jumped = true;
		}
		if(Input.GetAxisRaw("Horizontal") == 0)
		{
			stateMachine.SetState(new IdleState(stateMachine, ped));
		}
		else
		{
			stateMachine.SetState(new WalkingState(stateMachine, ped));
		}
		ped.Walk();
	}

	public override void FixedUpdateState()
	{
		ped.Animator.SetBool("isAirborn", false);
		if(ped.jumped)
		{
			ped.Jump();
		}
		ped.Walk();
	}

	public override void ExitState()
	{
		
	}
}
