/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control Cinder.
* Attaches this to the cinder object.
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CinderScript : Ped
{
	// Classes
	AI cinderAI;

	// Global Variables
	[Header("Cinder Settings")]
	[SerializeField]
	private PedNames _name = PedNames.Cinder;
	[SerializeField]
	private Direction _startMovementDirection = Direction.Left;
	[SerializeField]
	private Direction _startFaceDirection = Direction.Left;
	[SerializeField]
	private bool _blockAI = false;
	[SerializeField][Range(0.1f, 7.0f)]
	private float _speed = 1f, _alertedSpeed = 5, _timeToMorph = 0.7f;
	private float _jumpForce = 7f;
	private float _groundCheckRadius = 0.1f;
	private float _sideCheckRadius = 0.4f;

	// ==============================================================
	// Monobehaviour Methods (In order of execution)
	// ==============================================================

	protected override void Awake()
	{
		base.Awake();

		cinderAI = GetComponent<AI>();
		Assert.IsNotNull(cinderAI);
	}

	protected override void Start()
	{
		base.Start();

		pedType = PedType.Enemy;
		Name = _name.ToString();
		JumpForce = _jumpForce;
		Assert.AreNotEqual(_jumpForce, 0);
		SideCheckRadius = _sideCheckRadius;
		GroundCheckRadius = _groundCheckRadius;
		MovementDirection = (int)_startMovementDirection;
		FaceDirection = (int)_startFaceDirection;

		if(_blockAI)
		{
			MovementDirection = (int)Direction.Idle;
			cinderAI.enabled = false;
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
			if(!HasMorphed && !IsAlerted)
			{
				cinderAI.DetectPlayer(AI.LookDirection.Down);
				cinderAI.AvoidLedgesAndWalls();
			}
			
			if(IsAlerted)
			{
				StartCoroutine(AttemptToSquashPlayer());
			}
		}
	}

	// ============================================================
	// Cinder Related tasks.
	// ============================================================

	private IEnumerator AttemptToSquashPlayer()
	{
		Assert.IsTrue(IsAlerted);
		Speed = _alertedSpeed;
		MovementDirection = (int)FaceDirection;
		if
		(
			!cinderAI.HasReachedLedgeOnLeftSide && cinderAI.HasReachedLedgeOnRightSide ||
			cinderAI.HasReachedLedgeOnLeftSide && !cinderAI.HasReachedLedgeOnRightSide 
			&& IsGrounded
		)
		{
			Jumped = true;
			Assert.AreNotEqual(_timeToMorph, 0);
			yield return new WaitForSeconds(_timeToMorph);
			if(!IsAlerted)
			{
				yield break;
			}
			SetPedState(States.Block);
		}
	}
}
