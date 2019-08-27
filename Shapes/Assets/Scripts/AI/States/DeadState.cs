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
		//Debug.Log(ped.Name + " is in dead state");
		//SubscribeToInteractionEvents();
		ped.StartCoroutine(FallOffScreen());
		ped.IsAbleToMove = false;
	}

	public override void UpdateState()
	{
		Debug.Log(ped.Name + " is in a dead state");
	}

	public override void FixedUpdateState()
	{
		
	}

	public override void ExitState()
	{
		//UnsubscribeToInteractionEvents();
	}

	// ==============================================================
	// Events - What happens when an event triggers during this state? 
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += ped.Destroy;
		ped.HasHitBallState += FallOff;
		ped.HasHitHorizontalShieldState += FallOff;
		ped.HasHitVerticalShieldState += FallOff;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitBlockState -= ped.Destroy;
		ped.HasHitBallState -= FallOff;
		ped.HasHitHorizontalShieldState -= FallOff;
		ped.HasHitVerticalShieldState -= FallOff;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void FallOff()
	{
		ped.StartCoroutine(FallOffScreen());
	}

	private void Vanish()
	{
		// Play Sound
		ped.Destroy();
	}

	private IEnumerator FallOffScreen()
	{
		//ped.Rigidbody2D.AddForce(ped.transform.up * 180f);
		ped.Animator.enabled = false;
		//ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		//yield return new WaitForEndOfFrame();	// Detect 'OnCollisionExit2D' before disabling the colliders. 
		foreach(Collider2D collider in ped.GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(4);
		ped.Destroy();
	}
}