using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AegisScript : Ped
{
	AI dynamoAI;

	[Header("Aegis Settings")]
	[SerializeField]
	private string _name = "Aegis";
	[SerializeField]
	private bool _blockAI = false;
	[SerializeField][Range(0.1f, 7.0f)]
	private float _speed = 0.1f, _alertedRange = 5.8f;
	private float _groundCheckRadius = 0.2f;
	private float _sideCheckRadius = 0.4f;

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
		SideCheckRadius = _sideCheckRadius;
		GroundCheckRadius = _groundCheckRadius;
		BlockAI = _blockAI;
		dynamoAI = GetComponent<AI>();
		if(BlockAI)
		{
			SetPedState(States.Idle);
			dynamoAI.enabled = false;
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

		Speed = _speed;
		if(!BlockAI)
		{
			if(!HasMorphed)
			{
				//dynamoAI.DetectPlayer(AI.LookDirection.Up);
				dynamoAI.AvoidLedgesAndWalls();
			}
			
			//if(IsAlerted)
			//{
				if(DistanceBetweenPedAndPlayer <= _alertedRange)
				{
					IsAlerted = true;
					SetPedState(States.HorizontalShield);
				}
				else
				{
					IsAlerted = false;
				}
			//}
		}
	}

	protected override void FixedUpdate()
	{
		base.FixedUpdate();
	}
}
