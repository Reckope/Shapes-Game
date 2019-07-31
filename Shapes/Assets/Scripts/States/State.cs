using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class State
{
	protected Ped ped;

	public abstract void EnterState();
	public abstract void UpdateState();
	public virtual void FixedUpdateState(){}	// Optional in case there's no physics involved.  
	public abstract void ExitState();

	public State(Ped ped)
	{
		this.ped = ped;
	}
}
