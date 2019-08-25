using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoHorizontalShieldState : State
{
	public MorphIntoHorizontalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.gameObject.layer = LayerMask.NameToLayer("HorizontalShield");
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToHorizontalShield", true);
	}

	public override void UpdateState()
	{
		if (!ped.MorphToHorizontalShieldInput)
		{
			ped.ExitMorphState();
		}

		//if(ped.HasHitTheGround)
		//{
		//	ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			// Do other stuff (camera shake, sound etc)
		//}
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		ped.RevertLayerMask();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToHorizontalShield", false);
	}
}
