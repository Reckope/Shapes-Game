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
 
	// Need to check if null as the ped won't be in any previous state
	// when initially setting the state at the start. 
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

	// If other classes want to check what state this ped is in. 
	public string CurrentState
	{ 
		get{ 
			if(currentState != null)
			{
				return currentState.ToString();
			}
			else
			{
				return "NULL";
			}
		}
	}
}
