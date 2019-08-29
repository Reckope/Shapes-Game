/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* WORK IN PROGRESS
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamoScript : Ped
{
	AI dynamoAI;

	[Header("Dynamo Settings")]
	[SerializeField]
	private string _name = "Dynamo";
	[SerializeField]
	private bool blockAI = false;
	[SerializeField][Range(0.1f, 7.0f)]
	private float _speed = 0.1f, _alertedSpeed = 5, _morphToPlayerRange = 5.8f;
	private float _groundCheckRadius = 0.2f;
	private float _sideCheckRadius = 0.4f;

	protected override void Awake()
	{
		base.Awake();
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
		pedType = PedType.Enemy;
		Name = _name;
		SideCheckRadius = _sideCheckRadius;
		GroundCheckRadius = _groundCheckRadius;
		dynamoAI = GetComponent<AI>();
		if(blockAI)
		{
			SetPedState(States.Idle);
			dynamoAI.enabled = false;
		}
		else
		{
			MovementDirection = (int)Direction.Left;
		}
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		Speed = _speed;

		if(!blockAI)
		{
			if(!HasMorphed)
			{
				dynamoAI.DetectPlayer(AI.LookDirection.StraightAhead);
				dynamoAI.AvoidLedgesAndWalls();
			}
			
			if(IsAlerted)
			{
				Speed = _alertedSpeed;
				if(DistanceBetweenPedAndPlayer <= _morphToPlayerRange)
				{
					SetPedState(States.Ball);
				}
				else if(CollidedLeft || CollidedRight || dynamoAI.HasReachedLedgeOnLeftSide || dynamoAI.HasReachedLedgeOnRightSide)
				{
					SetPedState(States.Ball);
				}
			}
		}
	}
}
