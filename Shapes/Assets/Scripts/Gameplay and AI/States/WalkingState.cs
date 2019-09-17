/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds Walk.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingState : State
{
	// Global Variables
	private float walkingAnimationSpeed = 11f;
	private float flinchUpwardsForce = 430f;

	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public WalkingState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================

	public override void EnterState()
	{
		SubscribeToPedInteractionEvents();
		ped.Animator.SetBool("isWalking", true);
	}

	public override void UpdateState()
	{
		if(ped.IsGrounded)
		{
			ped.Animator.speed = ped.Speed - (ped.Speed / walkingAnimationSpeed);
		}
		else
		{
			ped.Animator.speed = 1;
		}
	}

	public override void ExitState()
	{
		UnsubscribeToPedInteractionEvents();
		ped.Animator.speed = 1;
		ped.Animator.SetBool("isWalking", false);
	}

	// ==============================================================
	// Events trigger certain methods exclusive to walking.
	// ==============================================================

	private void SubscribeToPedInteractionEvents()
	{
		ped.HasHitBallState += HitBall;
		ped.HasHitBlockState += HitBlock;
		ped.HasHitHorizontalShieldState += HitHorizontalShield;
		ped.HasHitVerticalShieldState += HitVerticalShield;
	}

	private void UnsubscribeToPedInteractionEvents()
	{
		ped.HasHitBallState -= HitBall;
		ped.HasHitBlockState -= HitBlock;
		ped.HasHitHorizontalShieldState -= HitHorizontalShield;
		ped.HasHitVerticalShieldState -= HitVerticalShield;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitBall()
	{
		// Sound
		// Public event: Name + died.
		ped.Rigidbody2D.AddForce(ped.transform.up * flinchUpwardsForce);
		ped.BounceAway();
		ped.TakeDamage(Ped.DamageType.Hit);
	}

	private void HitBlock()
	{
		ped.PlaySound(Ped.PedSounds.Splat, true, false, 0.8f);
		ped.TakeDamage(Ped.DamageType.Destroy);
	}

	private void HitHorizontalShield()
	{
		TakeOff();
	}

	private void HitVerticalShield()
	{
		TakeOff();
	}

	// ============================================================
	// Private Methods used by this state.
	// ============================================================

	private void TakeOff()
	{
		if(!ped.IsGrounded)
		{
			ped.Animator.SetTrigger("takeOff");
		}
		else
		{
			ped.BounceAway();
		}
	}
}
