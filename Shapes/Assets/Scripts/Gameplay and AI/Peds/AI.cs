/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to enable basic AI for any ped object this is attached to.
* This can easily be disabled by script if a designer wants to block AI for a ped.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(Ped))]
public class AI : MonoBehaviour
{
	// Classes
	Ped ped;

	// Enums
	public enum LookDirection
	{
		Up,
		StraightAhead,
		Down
	}

	// GameObjects / Transforms
	public Transform leftLedgeCheck;
	public Transform rightLedgeCheck;

	// Global variables
	[Header("AI Settings")]
	[SerializeField][Range(5f, 15.0f)]
	private float _visionDistance = 10;

	public bool HasReachedLedgeOnLeftSide
	{ 
		get { return !Physics2D.OverlapCircle (leftLedgeCheck.position, ped.GroundCheckRadius, ped.whatIsGround); } 
	}

	public bool HasReachedLedgeOnRightSide
	{ 
		get { return !Physics2D.OverlapCircle (rightLedgeCheck.position, ped.GroundCheckRadius, ped.whatIsGround); } 
	}

	// ==============================================================
	// Monobehaviour Methods (in order of execution)
	// ==============================================================

	protected void Awake()
	{
		ped = GetComponent<Ped>();
		Assert.IsNotNull(ped);
	}

	// ============================================================
	// Basic Tasks that other scrips can call.
	// ============================================================

	public void AvoidLedgesAndWalls()
	{
		if((ped.CollidedLeft && !ped.CollidedRight) || (HasReachedLedgeOnLeftSide && !HasReachedLedgeOnRightSide) && !ped.IsAlerted)
		{
			ped.MovementDirection = (int)Ped.Direction.Right;
		}
		else if((ped.CollidedRight && !ped.CollidedLeft) || (HasReachedLedgeOnRightSide && !HasReachedLedgeOnLeftSide) && !ped.IsAlerted)
		{
			ped.MovementDirection = (int)Ped.Direction.Left;
		}
	}

	public void DetectPlayer(LookDirection requestedDirection)
	{
		Vector2 visionDirection = Vector2.zero;
		Vector2 visionSpawn = transform.position;
		Vector2 left = ped.leftCheck.transform.position;
		Vector2 right = ped.rightCheck.transform.position;

		// The Raycast can't pass through objects with colliders, so
		// need to switch the spawn left to right. 
		if(ped.FaceDirection == (int)Ped.Direction.Left)
		{
			visionSpawn = left;
		}
		else if(ped.FaceDirection == (int)Ped.Direction.Right)
		{
			visionSpawn = right;
		}

		// Which direction should the raycast point?
		switch(requestedDirection)
		{
			case LookDirection.StraightAhead:
				if(visionSpawn == left)
				{
					visionDirection = Vector2.left;
				}
				else if(visionSpawn == right)
				{
					visionDirection = Vector2.right;
				}
			break;

			case LookDirection.Up:
				visionDirection = Vector2.up;
			break;

			case LookDirection.Down:
				visionDirection = Vector2.down;
			break;
		}

		RaycastHit2D lineOfSight = Physics2D.Raycast(visionSpawn, visionDirection, _visionDistance);

		if(lineOfSight.collider != null && lineOfSight.collider.gameObject.layer == LayerMask.NameToLayer(Ped.PedType.Player.ToString())){
			ped.IsAlerted = true;
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
