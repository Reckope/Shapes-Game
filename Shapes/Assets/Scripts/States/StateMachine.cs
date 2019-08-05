/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is where the given model (Derived Ped) transitions
* to other behavioral states through external input.
* Derived Ped > Ped > Statemachine (here) > State > SomeState
*/

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
		if(currentState != null)
		{
			currentState.ExitState();
		}
		currentState = newState;
		currentState.EnterState();
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

	public void LateUpdateState()
	{
		if (currentState != null)
		{
			currentState.LateUpdateState();
		}
	}

	// ============================================================
	// Get current state.
	// ============================================================

	public State GetCurrentState(){ return currentState; }
}
