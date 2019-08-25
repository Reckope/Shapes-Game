﻿using System;
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
		ped.IsAbleToMove = false;
		if(ped.pedType == Ped.PedType.Enemy)
		{
			//EnemyDie();
			if(ped.HasHitBlockState)
			{
				Debug.Log("SQUAH");
				ped.Destroy();
			}
			else
			{
				ped.StartCoroutine(FallOffScreen());
			}
		}
		else
		{
			ped.StartCoroutine(FallOffScreen());
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

	private IEnumerator FallOffScreen()
	{
		ped.Animator.enabled = false;
		yield return new WaitForEndOfFrame();	// Detect 'OnCollisionExit2D' before disabling the colliders. 
		foreach(Collider2D collider in ped.GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(4);
		ped.Destroy();
	}

	private void Vanish()
	{
		// Play Sound
		ped.Destroy();
	}
}