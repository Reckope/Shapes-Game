using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	// Global Variables
	private State currentState;

	// ============================================================
	// Monobehaviour public methods to set the state. 
	// ============================================================

	public StateMachine(State state)
	{
		this.currentState = state;
	}
 
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

	// ============================================================
	// Get current state.
	// ============================================================

	public State GetCurrentState(){ return currentState; }
}
