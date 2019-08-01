using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	private State currentState;
	//private Ped ped;
 
	public void SetState (State newState)
	{
		if (currentState != null)
		{
			currentState.ExitState();
		}
		currentState = newState;
		if (currentState != null)
		{
			currentState.EnterState();
		}
	}

	public void UpdateState()
	{
		if (currentState != null)
		{
			currentState.UpdateState();
		}
	}

	public void FixedUpdateState()
	{
		if (currentState != null)
		{
			currentState.FixedUpdateState();
		}
	}

	public StateMachine(State state)
	{
		this.currentState = state;
	}

	public void test()
	{
		Debug.Log("Test");
	}
}
