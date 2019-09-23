/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control Dynamo.
* Attaches this to the Dynamo object.
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class DynamoScript : Ped
{
	// Classes
	AI dynamoAI;

	// Global variables
	[Header("Dynamo Settings")]
	[SerializeField]
	private PedNames _name = PedNames.Dynamo;
	[SerializeField]
	private Direction _startMovementDirection = Direction.Left;
	[SerializeField]
	private Direction _startFaceDirection = Direction.Left;
	[SerializeField]
	private bool _blockAI = false;
	[SerializeField][Range(0.1f, 7.0f)]
	private float _speed = 0.1f, _alertedSpeed = 5, _morphToPlayerRange = 5.8f;
	private float _groundCheckRadius = 0.2f;
	private float _sideCheckRadius = 0.4f;

	// ==============================================================
	// Monobehaviour Methods (in order of execution).
	// ==============================================================

	protected override void Awake()
	{
		base.Awake();

		dynamoAI = GetComponent<AI>();
		Assert.IsNotNull(dynamoAI);
	}

	protected override void Start()
	{
		base.Start();

		pedType = PedType.Enemy;
		Name = _name.ToString();
		SideCheckRadius = _sideCheckRadius;
		GroundCheckRadius = _groundCheckRadius;

		if(_blockAI)
		{
			MovementDirection = (int)Direction.Idle;
			dynamoAI.enabled = false;
		}
		else
		{
			MovementDirection = (int)_startMovementDirection;
		}
		FaceDirection = (int)_startFaceDirection;
	}

	protected override void Update()
	{
		base.Update();

		Speed = _speed;

		if(!_blockAI)
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
