﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : State
{
	private Animator anim;

	public IdleState(StateMachine stateMachine) : base(stateMachine)
	{

	}

	override public void EnterState()
	{
		Debug.Log("I Have Become IDLE!");
	}

	override public void UpdateState()
	{
		Debug.Log("I Am Updating IDLE!");
	}

	override public void FixedUpdateState()
	{
		Debug.Log("I Am Fixed Updating IDLE!");
	}

	override public void ExitState()
	{
		Debug.Log("I Have Finished Being IDLE!");
	}
}
