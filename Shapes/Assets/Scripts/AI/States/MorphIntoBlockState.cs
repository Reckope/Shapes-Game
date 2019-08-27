using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBlockState : State
{
	public MorphIntoBlockState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.ChangeTag(Ped.States.Block);
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
		ped.Animator.SetBool("morphToBlock", true);
	}

	public override void UpdateState()
	{
		ped.transform.rotation = Quaternion.identity;
	}

	public override void ExitState()
	{
		if(!ped.IsDead)
		{
			ped.RevertTag();
		}
		UnsubscribeToInteractionEvents();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBlock", false);
	}

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += ped.Die;
		ped.HasHitHorizontalShieldState += ped.Die;
		ped.HasHitVerticalShieldState += HitByVerticalShield;
		ped.HasHitGround += HasHitTheGround;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitHorizontalShieldState -= ped.Die;
		ped.HasHitVerticalShieldState -= HitByVerticalShield;
		ped.HasHitGround -= HasHitTheGround;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitByVerticalShield()
	{
		ped.StartCoroutine(ExitBlockState());
	}

	private void PlayerControls()
	{
		if(!ped.MorphToBlockInput)
		{
			ped.ExitMorphState();
		}
	}

	private void HasHitTheGround()
	{
		if(!ped.IsDead && ped.pedType == Ped.PedType.Enemy)
		{
			// Do other stuff (camera shake, sound etc)
			Debug.Log("HIT GROUND");
			ped.Die();
		}
		else if(ped.pedType == Ped.PedType.Player)
		{
			ped.StartCoroutine(ExitBlockState());
		}
	}

	private void EnemyControls()
	{

		if(!ped.IsDead)
		{
			// Do other stuff (camera shake, sound etc)
			Debug.Log("HIT GROUND");
			ped.Die();
		}
	}

	private IEnumerator ExitBlockState()
	{
		//ped.Rigidbody2D.AddForce(Vector2.up * 20);
		yield return new WaitForSeconds(0.5f);
		ped.ExitMorphState();
	}
}
