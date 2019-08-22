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
	private bool alerted;
	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, _groundCheckRadius = 0.2f;

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
		Speed = _speed;
		JumpForce = _jumpForce;
		GroundCheckRadius = _groundCheckRadius;
		dynamoAI = GetComponent<AI>();
		if(blockAI)
		{
			MovementDirection = (int)Direction.Idle;
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
		Debug.Log(CollidedRight);
		if(dynamoAI.DetectPlayer() && !blockAI)
		{
			Speed = 5;
			Debug.Log("Can See you");
			if(CollidedRight)
			{
				Debug.Log("Collided?");
				SetPedState(States.Ball);
			}
			else if(DistanceBetweenPlayerAndEnemy() <= 5.8)
			{
				SetPedState(States.Ball);
			}
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	private float DistanceBetweenPlayerAndEnemy(){
        float distance;
        if(player != null){
            distance = Vector2.Distance(player.transform.position, gameObject.transform.position);
        }
        else{
            distance = 0;
        }
        return distance;
    }
}
