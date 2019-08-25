using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinderScript : Ped
{
	AI cinderAI;

	[Header("Cinder Settings")]
	[SerializeField]
	private string _name = "Cinder";
	[SerializeField]
	private bool blockAI = false;
	[SerializeField][Range(0.1f, 7.0f)]
	private float _speed = 0.1f, _alertedSpeed = 5, _jumpForce = 6;
	private float _groundCheckRadius = 0.1f;
	private float _sideCheckRadius = 0.4f;
	private bool _jumped = false;

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
		JumpForce = _jumpForce;
		SideCheckRadius = _sideCheckRadius;
		GroundCheckRadius = _groundCheckRadius;
		cinderAI = GetComponent<AI>();
		MovementDirection = (int)Ped.Direction.Left;
	}

	// Update is called once per frame
	protected override void Update()
	{
		base.Update();
		Speed = _speed;

		if(!BlockAI && !IsDead)
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

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}

	private IEnumerator AttemptToSquashPlayer()
	{
		Speed = _alertedSpeed;
		if
		(
			!cinderAI.HasReachedLedgeOnLeftSide && cinderAI.HasReachedLedgeOnRightSide ||
			cinderAI.HasReachedLedgeOnLeftSide && !cinderAI.HasReachedLedgeOnRightSide 
			&& IsGrounded
		)
		{
			HasJumped = true;
			yield return new WaitForSeconds(0.7f);
			SetPedState(States.Block);
		}
	}
}
