using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadState : State
{
	// Global Variables
	private int secondsBeforeDestroyingPed = 4;
	
	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public DeadState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================

	public override void EnterState()
	{
		//Debug.Log(ped.Name + " is in dead state");
		//SubscribeToInteractionEvents();
		ped.StartCoroutine(FallOffScreen());
		ped.IsAbleToMove = false;
	}

	public override void ExitState()
	{
		//UnsubscribeToInteractionEvents();
	}

	// ==============================================================
	// Events trigger certain methods exclusive to death :/.
	// ==============================================================

	private void FallOff()
	{
		ped.StartCoroutine(FallOffScreen());
	}

	private IEnumerator FallOffScreen()
	{
		ped.Animator.enabled = false;
		foreach(Collider2D collider in ped.GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(secondsBeforeDestroyingPed);
		ped.Destroy();
	}
}