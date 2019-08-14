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

public class BallEnemy : Ped
{
	[Header("Ball Enemy Components and Variables")]
	public Transform leftLedgeCheck;
	public Transform rightLedgeCheck;

	private bool NoLeftLedgeDetected 
	{ 
		get { return Physics2D.OverlapCircle (leftLedgeCheck.position, GroundCheckRadius, whatIsGround); } 
	}

	private bool NoRightLedgeDetected
	{ 
		get { return Physics2D.OverlapCircle (rightLedgeCheck.position, GroundCheckRadius, whatIsGround); } 
	}

	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, _groundCheckRadius = 0.2f;
	private int left = -1;
	private int right = 1;

	protected override void Awake()
	{
		base.Awake();
		Name = "Roller";
		Speed = _speed;
		JumpForce = _jumpForce;
		GroundCheckRadius = _groundCheckRadius;
		MovementDirection = left;
	}

	// Start is called before the first frame update
	protected override void Start()
	{
		base.Start();
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		if(!HasMorphed)
		{
			AvoidLedgesAndWalls();
		}
		if(Input.GetKeyDown("o"))
		{
			SetMorphState(MorphStates.Ball);
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	private void AvoidLedgesAndWalls()
	{
		if(CollidedLeft && !CollidedRight)
		{
			MovementDirection = right;
		}
		else if(CollidedRight && !CollidedLeft)
		{
			MovementDirection = left;
		}

		if(!NoLeftLedgeDetected)
		{
			MovementDirection = right;
		}
		else if (!NoRightLedgeDetected)
		{
			MovementDirection = left;
		}
	}
}
