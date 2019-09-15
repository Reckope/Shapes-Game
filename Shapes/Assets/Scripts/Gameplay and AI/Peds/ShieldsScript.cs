/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control both Aegis and Priwen.
* Attaches this to both of their objects.
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldsScript : Ped
{
	// Classes
	AI shieldAI;

	// Global Variables
	[Header("Shield Settings")]
	[SerializeField]
	private PedNames _name = PedNames.Aegis;
	[SerializeField]
	private Direction _startMovementDirection = Direction.Idle;
	[SerializeField]
	private bool _blockAI = false;
	[SerializeField][Range(0f, 7.0f)]
	private float _speed = 1f, _alertedRange = 5.8f;
	private float _groundCheckRadius = 0.2f;
	private float _sideCheckRadius = 0.4f;

	// ==============================================================
	// Monobehaviour Methods.
	// ==============================================================

	protected override void Awake()
	{
		base.Awake();

		shieldAI = GetComponent<AI>();
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
			shieldAI.enabled = false;
		}
		else
		{
			MovementDirection = (int)_startMovementDirection;
		}
	}

	// Both shield enemies morphs once the player is in range, regardless of 
	// whether or not they can see the player.
	protected override void Update()
	{
		base.Update();

		Speed = _speed;
		if(!_blockAI)
		{
			if(!HasMorphed)
			{
				shieldAI.AvoidLedgesAndWalls();
			}
			
			if(DistanceBetweenPedAndPlayer <= _alertedRange)
			{
				IsAlerted = true;
				if(_name == PedNames.Aegis)
				{
					SetPedState(States.HorizontalShield);
				}
				else
				{
					SetPedState(States.VerticalShield);
				}
			}
			else
			{
				IsAlerted = false;
			}
		}
	}
}
