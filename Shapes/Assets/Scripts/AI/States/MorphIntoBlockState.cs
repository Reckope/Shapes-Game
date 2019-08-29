using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBlockState : State
{
	public MorphIntoBlockState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	private float pos;
	private float downwardForce = 0.1f;
	private float maxForce = 15f;
	private bool hitShield;
	
	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.ChangeTag(Ped.States.Block);
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX;
		ped.Animator.SetBool("morphToBlock", true);
		pos = ped.transform.position.x;
	}

	public override void UpdateState()
	{
		ped.transform.rotation = Quaternion.identity;
		ped.transform.position = new Vector2(pos, ped.transform.position.y);
	}

	public override void FixedUpdateState()
	{
		ped.transform.rotation = Quaternion.identity;
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
		ped.HasHitBlockState += ped.TakeDamage;
		ped.HasHitHorizontalShieldState += HandleTakenDamage;
		ped.HasHitVerticalShieldState += Exit;
		ped.HasHitGround += HandleHitGround;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitBlockState -= ped.TakeDamage;
		ped.HasHitHorizontalShieldState -= HandleTakenDamage;
		ped.HasHitVerticalShieldState -= Exit;
		ped.HasHitGround -= HandleHitGround;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void Exit()
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

	private void HandleTakenDamage()
	{
		hitShield = true;
		if(!ped.IsDead && ped.pedType == Ped.PedType.Enemy)
		{
			// Do other stuff (camera shake, sound etc)
			ped.Die();
		}
		else if(ped.pedType == Ped.PedType.Player)
		{
			ped.TakeDamage();
			ped.StartCoroutine(ExitBlockState());
		}
	}

	private void HandleHitGround()
	{
		if(!ped.IsDead && ped.pedType == Ped.PedType.Enemy)
		{
			// Do other stuff (camera shake, sound etc)
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
			ped.TakeDamage();
		}
	}

	private IEnumerator ExitBlockState()
	{
		yield return new WaitForSeconds(0.5f);
		ped.ExitMorphState();
	}
}
