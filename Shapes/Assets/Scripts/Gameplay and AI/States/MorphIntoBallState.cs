/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when peds have
* morphed into a ball.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBallState : State
{
	// Global Variables
	private float ballSpeed = 4f;
	private bool isAbleToAddForce;
	private float maxForce = 12f;
	private bool soundIsPlaying;

	// Call the constructure from SetState (StateMachine.cs), then override all of the peds Monobehaviour methods (Ped.cs).
	public MorphIntoBallState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }

	// ==============================================================
	// Abstract and virtual methods from State.cs
	// ==============================================================
	
	public override void EnterState()
	{
		SubscribeToInteractionEvents();
		ped.ChangeTag(Ped.States.Ball);
		isAbleToAddForce = true;
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Speed += ballSpeed;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBall", true);
	}

	public override void UpdateState()
	{	
		if(!ped.IsDead)
		{
			if(ped.pedType == Ped.PedType.Player)
			{
				PlayerControls();
			}
			else
			{
				if(ped.player != null)
				{
					MoveTowardsPlayer();
				}
			}
		}
	}

	public override void FixedUpdateState()
	{
		if(ped.IsGrounded)
		{
			AddForce();
		}
	}

	public override void ExitState()
	{
		UnsubscribeToInteractionEvents();
		if(!ped.IsDead)
		{
			ped.IsAbleToJump = true;
			ped.IsAbleToMove = true;
			ped.HasMorphed = false;
			ped.Speed -= ballSpeed;
			ped.transform.rotation = Quaternion.identity;
			ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
			ped.RevertTag();
			ped.Animator.SetBool("morphToBall", false);
			ped.PlaySound(Ped.PedSounds.BallRolling, false, false, 0.3f);
		}
	}

	// ==============================================================
	// Events trigger certain methods exclusive to the ball.
	// ==============================================================

	private void SubscribeToInteractionEvents()
	{
		ped.HasHitBlockState += HitBlock;
		ped.HasHitBallState += HitBall;;
		ped.HasHitHorizontalShieldState += HitHorizontalShield;
		ped.HasHitVerticalShieldState += HitVerticalShield;
		ped.HasHitGround += PlayRollingSound;
	}

	private void UnsubscribeToInteractionEvents()
	{
		ped.HasHitBlockState -= HitBlock;
		ped.HasHitBallState -= HitBall;
		ped.HasHitHorizontalShieldState -= HitHorizontalShield;
		ped.HasHitVerticalShieldState -= HitVerticalShield;
		ped.HasHitGround -= PlayRollingSound;
	}

	// ==============================================================
	// Methods that events subscribe to.
	// ==============================================================

	private void HitBall()
	{
		BounceBack();
	}

	private void HitBlock()
	{
		ped.PlaySound(Ped.PedSounds.Splat, true, false, 0.8f);
		ped.TakeDamage(Ped.DamageType.Destroy);
	}

	private void HitHorizontalShield()
	{
		if(!ped.IsGrounded)
		{
			ped.TakeDamage(Ped.DamageType.Hit);
		}
		else
		{
			BounceBack();
		}
	}

	private void HitVerticalShield()
	{
		ped.TakeDamage(Ped.DamageType.Hit);
	}

	// ============================================================
	// Private Methods used by this state.
	// ============================================================

	// Give the player / ped some sort of control, but not as much as walking normally.
	private void AddForce()
	{
		if(isAbleToAddForce && !ped.IsDead && ped.Rigidbody2D.velocity.x > -maxForce && ped.Rigidbody2D.velocity.x < maxForce)
		{
			Vector2 movement = new Vector2 (ped.MovementDirection, 0);
			ped.Rigidbody2D.AddForce(movement * ballSpeed);
		}
	}

	private void PlayerControls()
	{
		if(!ped.MorphToBallInput && !ped.CollidedTop && (!ped.CollidedLeft || !ped.CollidedRight))
		{
			ped.ExitMorphState();
		}
	}

	private void MoveTowardsPlayer()
	{
		if(ped.player.transform.position.x < ped.transform.position.x)
		{
			ped.MovementDirection = (int)Ped.Direction.Left;
		}
		else if(ped.player.transform.position.x > ped.transform.position.x)
		{
			ped.MovementDirection = (int)Ped.Direction.Right;
		}
	}

	private void BounceBack()
	{
		Vector2 bounceDirection = new Vector2(-(int)ped.MovementDirection, 0);
		float bounceAwayForce = 120f;

		ped.Rigidbody2D.AddForce(bounceDirection * bounceAwayForce);
	}

	private void PlayRollingSound()
	{
		if(soundIsPlaying)
		{
			return;
		}
		else
		{
			ped.PlaySound(Ped.PedSounds.BallRolling, true, true, 0.3f);
			soundIsPlaying = true;
		}
	}
}