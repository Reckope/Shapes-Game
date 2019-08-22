using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
	public DeadState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped)
	{

	}

	public override void EnterState()
	{
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		if(ped.pedType == Ped.PedType.Enemy)
		{
			//EnemyDie();
			ped.StartCoroutine(EnemyDie());
		}
		else
		{
			// Do player stuff
		}
	}

	public override void UpdateState()
	{
		
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		
	}

	private IEnumerator EnemyDie()
	{
		ped.Animator.enabled = false;
		foreach(Collider2D collider in ped.GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(2);
		ped.Destroy();
	}
}