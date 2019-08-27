using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoVerticalShieldState : State
{
	public MorphIntoVerticalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.ChangeLayerMask(Ped.States.VerticalShield);
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToVerticalShield", true);
		//ped.Rigidbody2D.bodyType = RigidbodyType2D.Static;
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
			ped.RevertLayerMask();
		}
		//ped.Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToVerticalShield", false);
	}

	private void PlayerControls()
	{
		if (!ped.MorphToVerticalShieldInput)
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
