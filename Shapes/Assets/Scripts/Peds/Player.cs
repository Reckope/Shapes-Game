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
	// Global Variables
	[SerializeField][Range(0.1f, 10.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f;

	protected override void Awake()
	{
		base.Awake();
		SetName("Morphy");
		SetSpeed(_speed);
		SetJumpForce(_jumpForce);
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
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}
