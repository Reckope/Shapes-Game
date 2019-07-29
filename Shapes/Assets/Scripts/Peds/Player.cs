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
	// Components
	private Rigidbody2D rb2d;
	private Collider2D Collider2D;
	private Animator anim;

	// Global Variables
	[Header("Player Settings")]
	[SerializeField][Range(0.1f, 10.0f)]
	private float _acceleration = 0.1f;
	[SerializeField][Range(0.1f, 10.0f)]
	private float _maxSpeed = 0.1f;

	// Start is called before the first frame update
	private void Awake()
	{
		setName("Morphy");
		setAcceleration(_acceleration);
		setMaxSpeed(_maxSpeed);
	}

	private void Start()
	{
		Collider2D = GetComponent<Collider2D>();
		rb2d = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	// Update is called once per frame
	private void Update()
	{
		MovePlayer();
	}

	private void FixedUpdate()
	{

	}

	private void MovePlayer()
	{
		float moveHorizontal;

		rb2d.bodyType = RigidbodyType2D.Dynamic;
		//grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
		moveHorizontal = Input.GetAxisRaw("Horizontal");
		Vector2 movement = new Vector2 (moveHorizontal, 0);
		if((rb2d.velocity.x >= -_maxSpeed && rb2d.velocity.x <= _maxSpeed)){
			rb2d.AddForce(movement * _acceleration);
		}
		if(rb2d.velocity.x > 0)
		{
			anim.Play("WalkRight");
		}
		else if(rb2d.velocity.x < 0)
		{
			anim.Play("WalkLeft");
		}
		else{
			anim.Play("Idle");
		}
	}
}
