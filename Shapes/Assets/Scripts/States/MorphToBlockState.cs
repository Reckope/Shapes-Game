using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphToBlockState : State
{
	public MorphToBlockState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
		ped.Animator.SetBool("morphToBlock", true);
	}

	public override void UpdateState()
	{
		if (!ped.MorphToBlockInput && ped.HasHitTheGroundWhileMorphed)
		{
			ped.ExitMorphState();
		}

		if(ped.HasHitTheGroundWhileMorphed)
		{
			//ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			//Debug.Log("FROZEN");
			// Do other stuff (camera shake, sound etc)
		}
	}

	public override void ExitState()
	{
		ped.HasHitTheGroundWhileMorphed = false;
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBlock", false);
	}
}
