/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds have
* morphed into a vertical shield.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoVerticalShieldState : State
{
	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public MorphIntoVerticalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================
	
	public override void EnterState()
	{
		SubscribeToPedInteractionEvents();
		ped.ChangeTag(Ped.States.VerticalShield);
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToVerticalShield", true);
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

	public override void ExitState()
	{
		UnsubscribeToPedInteractionEvents();
		if(!ped.IsDead)
		{
			ped.RevertTag();
			ped.IsAbleToJump = true;
			ped.IsAbleToMove = true;
			ped.HasMorphed = false;
			ped.transform.rotation = Quaternion.identity;
			ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
			ped.Animator.SetBool("morphToVerticalShield", false);
		}
	}

	// ==============================================================
	// Events trigger certain methods exclusive to the Vertical Shield.
	// ==============================================================

	private void SubscribeToPedInteractionEvents()
	{
		ped.HasHitBlockState += HitBlock;
	}

	private void UnsubscribeToPedInteractionEvents()
	{
		ped.HasHitBlockState -= HitBlock;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitBlock()
	{
		ped.PlaySound(Ped.PedSounds.Splat, true, false, 0.8f);
		ped.TakeDamage(Ped.DamageType.Destroy);
	}

	// ============================================================
	// Private Methods used by this state.
	// ============================================================

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