/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds have
* morphed into a horizontal shield.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoHorizontalShieldState : State
{
	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public MorphIntoHorizontalShieldState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================
	
	public override void EnterState()
	{
		SubscribeToInteractionEvents();
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

	public override void ExitState()
	{
		UnsubscribeToInteractionEvents();
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

	// ==============================================================
	// Events trigger certain methods exclusive to the Horizontal Shield.
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += HitBlock;
		ped.HasHitBallState += HitBall;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitBlockState -= HitBlock;
		ped.HasHitBallState -= HitBall;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitBlock()
	{
		// Event Data
	}

	private void HitBall()
	{
		if(ped.EnemyCollidedTop && (!ped.EnemyCollidedLeft || !ped.EnemyCollidedRight))
		{
			// Data
			// Sound
		}
		else
		{
			ped.TakeDamage(Ped.DamageType.Hit);
		}
	}

	// ============================================================
	// Private Methods used by this state.
	// ============================================================

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
