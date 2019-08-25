/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* Here we define the abstract class for the state. Every bit of behaviour
* that is state dependant becomes a virutal method. 
* Derived Ped > Ped > Statemachine > State (here) > SomeState
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected StateMachine stateMachine;
	protected Ped ped;

	public abstract void EnterState();
	public virtual void UpdateState(){}			// Optional
	public virtual void FixedUpdateState(){}	// Optional
	public virtual void LateUpdateState(){}		// Optional
	public abstract void ExitState();

	// We take the state machine and ped in the constructor of our state class and keep it in 
	// a protected variable so it’s available to our state classes.
	public State(StateMachine stateMachine, Ped ped)
	{
		this.stateMachine = stateMachine;
		this.ped = ped;
	}
}