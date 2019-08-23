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

	public Transform leftLedgeCheck;
	public Transform rightLedgeCheck;

	[SerializeField][Range(5f, 15.0f)]
	private float visionDistance = 10;


	public bool ReachedLedgeOnLeftSide
	{ 
		get { return !Physics2D.OverlapCircle (leftLedgeCheck.position, ped.GroundCheckRadius, ped.whatIsGround); } 
	}

	public bool ReachedLedgeOnRightSide
	{ 
		get { return !Physics2D.OverlapCircle (rightLedgeCheck.position, ped.GroundCheckRadius, ped.whatIsGround); } 
	}

	protected void Start()
	{
		ped = GetComponent<Ped>();
	}

	protected void Update()
	{
		if(!ped.HasMorphed)
		{
			DetectPlayer();
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
		if((ped.CollidedLeft && !ped.CollidedRight) || (ReachedLedgeOnLeftSide && !ReachedLedgeOnRightSide) && !ped.IsAlerted)
		{
			ped.MovementDirection = (int)Ped.Direction.Right;
		}
		else if((ped.CollidedRight && !ped.CollidedLeft) || (ReachedLedgeOnRightSide && !ReachedLedgeOnLeftSide) && !ped.IsAlerted)
		{
			ped.MovementDirection = (int)Ped.Direction.Left;
		}
	}

	// ============================================================
	// Player Related tasks.
	// ============================================================

	public void DetectPlayer()
	{
		Vector2 lookDirection;
		var raySpawn = transform.position;
		var left = ped.leftCheck.transform.position;
		var right = ped.rightCheck.transform.position;

		if(ped.MovementDirection == (int)Ped.Direction.Left)
		{
			lookDirection = Vector2.left;
			raySpawn = left;
		}
		else
		{
			lookDirection = Vector2.right;
			raySpawn = right;
		}
		RaycastHit2D lineOfSight = Physics2D.Raycast(raySpawn, lookDirection, 10);

		if(lineOfSight.collider != null && lineOfSight.collider.name == "Player"){
			ped.IsAlerted = true;
		}
		else
		{
			ped.IsAlerted = false;
		}
	}

	public void MoveTowardsPlayer()
	{
		if(ped.player.transform.position.x < transform.position.x)
		{
			ped.MovementDirection = (int)Ped.Direction.Left;
		}
		else if(ped.player.transform.position.x > transform.position.x)
		{
			ped.MovementDirection = (int)Ped.Direction.Right;
		}
	}
}
