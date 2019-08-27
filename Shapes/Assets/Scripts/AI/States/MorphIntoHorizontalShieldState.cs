using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoHorizontalShieldState : State
{
	public MorphIntoHorizontalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.ChangeTag(Ped.States.HorizontalShield);
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToHorizontalShield", true);
	}

	public override void UpdateState()
	{
		if(ped.pedType == Ped.PedType.Player)
		{
			PlayerControls();
		}
		else
		{
			EnemyControls();
		}
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		if(!ped.IsDead)
		{
			ped.RevertTag();
		}
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToHorizontalShield", false);
	}

	private void PlayerControls()
	{
		if (!ped.MorphToHorizontalShieldInput)
		{
			ped.ExitMorphState();
		}
	}

	private void EnemyControls()
	{
		if(!ped.IsAlerted)
		{
			ped.ExitMorphState();
		}
	}
}
