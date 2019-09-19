/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds are dead.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

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
		//SubscribeToInteractionEvents();
		ped.StartCoroutine(FallOffScreen());
		ped.IsAbleToMove = false;
	}

	public override void ExitState()
	{
		//UnsubscribeToInteractionEvents();
	}

	// ==============================================================
	// Death Methods
	// ==============================================================

	private IEnumerator FallOffScreen()
	{
		ped.Animator.enabled = false;
		foreach(Collider2D collider in ped.GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(secondsBeforeDestroyingPed);
		if(ped != null && ped.pedType != Ped.PedType.Player)
		{
			ped.Destroy();
		}
	}
}