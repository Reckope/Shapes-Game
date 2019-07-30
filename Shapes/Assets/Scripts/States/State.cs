using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected Ped ped;

	public abstract void UpdateState();	// A state needs to be updated at all times. 

	public virtual void HandleInput(){}	// Only handle input if the object needs to. 
	public virtual void OnStateEnter(){}
	public virtual void OnStateExit(){}

	public State(Ped ped)
	{
		this.ped= ped;
	}
}
