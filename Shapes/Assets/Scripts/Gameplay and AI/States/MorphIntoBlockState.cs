/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds have
* morphed into a block.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBlockState : State
{
	// Global Variables
	private float position;
	private float downwardForce = 0.1f;
	private float maxForce = 15f;
	private bool hitShield;

	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public MorphIntoBlockState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================
	
	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.ChangeTag(Ped.States.Block);
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
		ped.Animator.SetBool("morphToBlock", true);
		position = ped.transform.position.x;
	}

	public override void UpdateState()
	{
		ped.transform.rotation = Quaternion.identity;
		ped.transform.position = new Vector2(position, ped.transform.position.y);
	}

	public override void FixedUpdateState()
	{
		if(!hitShield)
		{
			ped.Rigidbody2D.velocity = Vector2.down * downwardForce;
			if(downwardForce < maxForce)
			{
				downwardForce ++;
			}
		}
	}

	public override void ExitState()
	{
		UnsubscribeToInteractionEvents();
		if(!ped.IsDead)
		{
			ped.RevertTag();
			ped.IsAbleToJump = true;
			ped.IsAbleToMove = true;
			ped.HasMorphed = false;
			ped.transform.rotation = Quaternion.identity;
			ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
			ped.Animator.SetBool("morphToBlock", false);
		}
	}

	// ==============================================================
	// Events trigger certain methods exclusive to the block.
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += HitBlock;
		ped.HasHitHorizontalShieldState += HitHorizontalShield;
		ped.HasHitGround += HitGround;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitBlockState -= HitBlock;
		ped.HasHitHorizontalShieldState -= HitHorizontalShield;
		ped.HasHitGround -= HitGround;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitBlock()
	{
		ped.TakeDamage(Ped.DamageType.Hit);
	}

	private void HitHorizontalShield()
	{
		hitShield = true;
		ped.TakeDamage(Ped.DamageType.Hit);
		ped.StartCoroutine(ExitBlockState());
	}

	private void HitGround()
	{
		if(!ped.IsDead)
		{
			ped.PlaySound(Ped.PedSounds.BlockToGround, true, false, 1f);
			if(ped.pedType == Ped.PedType.Enemy)
			{
				ped.TakeDamage(Ped.DamageType.Hit);
			}
			else if(ped.pedType == Ped.PedType.Player)
			{
				ped.StartCoroutine(ExitBlockState());
			}
		}
	}

	// ============================================================
	// Private Methods used by this state.
	// ============================================================

	private IEnumerator ExitBlockState()
	{
		yield return new WaitForSeconds(0.5f);
		ped.ExitMorphState();
	}
}
