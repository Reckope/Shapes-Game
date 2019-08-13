using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyOne : Ped
{

	protected override void Awake()
	{
		base.Awake();
		Name = "EnemyOne";
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		stateMachine.SetState(new IdleState(stateMachine, this));
		IsAbleToMove = true;
		IsAbleToJump = true;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}
