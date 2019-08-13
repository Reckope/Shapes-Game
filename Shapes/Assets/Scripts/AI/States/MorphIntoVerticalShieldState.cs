using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoVerticalShieldState : State
{
	public MorphIntoVerticalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
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
		if (!ped.MorphToVerticalShieldInput)
		{
			ped.ExitMorphState();
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
