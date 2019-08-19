﻿/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we can change / set what happens when the player has
* morphed into a ball.
* Derived Ped > Ped > Statemachine > State > SomeState (Here)
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphIntoBallState : State
{
	private float ballSpeed = 4f;
	private bool isAbleToAddForce;

	public MorphIntoBallState(StateMachine stateMachine, Ped ped) : base(stateMachine, ped) { }
	
	public override void EnterState()
	{
		ped.gameObject.layer = LayerMask.NameToLayer("Ball");
		isAbleToAddForce = true;
		ped.IsAbleToJump = false;
		ped.IsAbleToMove = false;
		ped.HasMorphed = true;
		ped.Speed += ballSpeed;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.None;
		ped.Animator.SetBool("morphToBall", true);
		Debug.Log(ped.Name + "Morphed");
	}

	public override void UpdateState()
	{
		if (ped.gameObject.tag == "Player")
		{
			PlayerControls();
		}
		else
		{
			MoveTowardsPlayer();
		}

		if(ped.HasHitVerticalShieldState)
		{
			isAbleToAddForce = false;
			ped.IsDead = true;
			HandleBallDeaths();
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
		ped.RevertLayerMask();
		ped.IsAbleToJump = true;
		ped.IsAbleToMove = true;
		ped.HasMorphed = false;
		ped.Speed -= ballSpeed;
		ped.transform.rotation = Quaternion.identity;
		ped.Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		ped.Animator.SetBool("morphToBall", false);
	}

	// ============================================================
	// Private Methods for when the ped has morphed into a ball.
	// ============================================================

	// Give the player / ped some sort of control, but not as much as walking normally.
	private void AddForce()
	{
		if(isAbleToAddForce && !ped.IsDead)
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
			ped.MovementDirection = -1;
		}
		else if(ped.player.transform.position.x > ped.transform.position.x)
		{
			ped.MovementDirection = 1;
		}
	}

	private void HandleBallDeaths()
	{
		if(ped.HasHitVerticalShieldState)
		{
			ped.StartCoroutine(DeathByShield());
		}
	}

	private IEnumerator DeathByShield()
	{
		ped.Animator.enabled = false;
		foreach(Collider2D collider in ped.GetComponentsInChildren<Collider2D>())
		{
			collider.enabled = false;
		}
		yield return new WaitForSeconds(2);
		ped.Destroy();
	}
}
