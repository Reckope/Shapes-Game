using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphToVerticalShieldState : State
{
	public MorphToVerticalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToVerticalShield", true);
	}

	public override void UpdateState()
	{
		if (!Input.GetKey("left"))
		{
			ped.TransitionAfterExitingState();
		}
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToVerticalShield", false);
	}
}
