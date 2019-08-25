using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBlockState : State
{
	public MorphIntoBlockState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.gameObject.layer = LayerMask.NameToLayer("Block");
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
		ped.Animator.SetBool("morphToBlock", true);
	}

	public override void UpdateState()
	{
		ped.transform.rotation = Quaternion.identity;
		Debug.Log(ped.Rigidbody2D.constraints);
		if(ped.pedType == Ped.PedType.Player)
		{
			PlayerControls();
		}
		else
		{
			EnemyControls();
		}

		if(ped.HasHitHorizontalShieldState)
		{
			ped.IsDead = true;
			ped.Die();
		}
	}

	public override void ExitState()
	{
		ped.HasHitGround = false;
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBlock", false);
	}

	private void PlayerControls()
	{
		if(ped.HasHitGround)
		{
			if(!ped.MorphToBlockInput)
			{
				ped.ExitMorphState();
			}
		}
	}

	private void EnemyControls()
	{

		if(ped.HasHitGround)
		{
			//ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeAll;
			//Debug.Log("FROZEN");
			// Do other stuff (camera shake, sound etc)
			ped.Die();
		}
	}
}
