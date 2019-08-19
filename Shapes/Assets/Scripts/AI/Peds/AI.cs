/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to enable basic UI for any ped object this is attached to.
* This can easily be disabled by script if a designer wants to block AI for a ped. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ped))]
public class AI : MonoBehaviour
{
	Ped ped;

	private enum Direction
	{
		left = -1,
		right = 1
	}
	Direction direction;

	public Transform leftLedgeCheck;
	public Transform rightLedgeCheck;


	private bool NoLeftLedgeDetected 
	{ 
		get { return Physics2D.OverlapCircle (leftLedgeCheck.position, ped.GroundCheckRadius, ped.whatIsGround); } 
	}

	private bool NoRightLedgeDetected
	{ 
		get { return Physics2D.OverlapCircle (rightLedgeCheck.position, ped.GroundCheckRadius, ped.whatIsGround); } 
	}

	protected void Start()
	{
		ped = GetComponent<Ped>();
		ped.MovementDirection = (int)Direction.left;
	}

	protected void Update()
	{
		if(!ped.HasMorphed)
		{
			AvoidLedgesAndWalls();
		}
	}

	protected void FixedUpdate()
	{
		
	}

	// ============================================================
	// Basic Tasks
	// ============================================================

	private void AvoidLedgesAndWalls()
	{
		if((ped.CollidedLeft && !ped.CollidedRight) || (!NoLeftLedgeDetected && NoRightLedgeDetected))
		{
			ped.MovementDirection = (int)Direction.right;
		}
		else if((ped.CollidedRight && !ped.CollidedLeft) || (!NoRightLedgeDetected && NoLeftLedgeDetected))
		{
			ped.MovementDirection = (int)Direction.left;
		}
	}

	// ============================================================
	// Player Related tasks.
	// ============================================================

	public bool CanSeePlayer()
	{
		return true;
	}

	public void MoveTowardsPlayer()
	{
		if(ped.player.transform.position.x < transform.position.x)
		{
			ped.MovementDirection = (int)Direction.left;
		}
		else if(ped.player.transform.position.x > transform.position.x)
		{
			ped.MovementDirection = (int)Direction.right;
		}
	}

}
