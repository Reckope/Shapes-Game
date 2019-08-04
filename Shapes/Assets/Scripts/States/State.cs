using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected StateMachine stateMachine;
	protected Ped ped;

	public abstract void EnterState();
	public abstract void UpdateState();
	public virtual void FixedUpdateState(){}	// Optional in case there's no physics involved.  
	public abstract void ExitState();

	// We take the state machine in the constructor of our state class and keep it in 
	// a protected variable so it’s available to our state classes.
	public State(StateMachine stateMachine, Ped ped)
	{
		this.stateMachine = stateMachine;
		this.ped = ped;
	}
}
