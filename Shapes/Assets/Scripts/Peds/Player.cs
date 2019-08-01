/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the player.
* Attach this to the player GameObject,
* WORK IN PROGRESS
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ped
{

	// GameObjects
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// Global Variables
	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, jumpForce = 0.1f;

	// Start is called before the first frame update
	private void Awake()
	{
		setName("Morphy");
		setSpeed(_speed);
	}

	private void Start()
	{
		SetRigidBody2D();
		SetCollider2D();
		SetAnimator();
		GetRigidBody2D().constraints = RigidbodyConstraints2D.FreezeRotation;
		stateMachine = GetComponent<StateMachine>();
		//SetPedState(new IdleState(this));
		stateMachine.SetState(new IdleState(stateMachine));
	}

	// Update is called once per frame
	private void Update()
	{
		stateMachine.UpdateState();
		MovePlayer();
	}

	private void FixedUpdate()
	{
		stateMachine.FixedUpdateState();
	}

	private void MovePlayer()
	{
		bool jump;
		bool grounded;
		float moveHorizontal;
		float groundCheckRadius = 0.3f;

		GetRigidBody2D().bodyType = RigidbodyType2D.Dynamic;
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		GetRigidBody2D().velocity = new Vector2(moveHorizontal * _speed, GetRigidBody2D().velocity.y);

		jump = Input.GetKeyDown(KeyCode.W);
		if(grounded && jump)
		{
			GetAnimator().SetTrigger("takeOff");
			GetRigidBody2D().velocity = Vector2.up * jumpForce;
		}

		if(grounded)
		{
			GetAnimator().SetBool("isJumping", false);
			if(moveHorizontal == 0)
			{
				GetAnimator().SetBool("isRunning", false);
			}
			else
			{
				GetAnimator().SetBool("isRunning", true);
			}
		}
		else
		{
			GetAnimator().SetBool("isJumping", true);
			GetAnimator().SetBool("isRunning", false);
		}

		if(moveHorizontal > 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		else if(moveHorizontal < 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
		else
		{
			GetAnimator().Play("Idle");
		}
	}
}
