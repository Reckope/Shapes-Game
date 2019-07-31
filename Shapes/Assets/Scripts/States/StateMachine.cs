using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
	private State currentState;
 
	public void SetState (State newState){
		currentState.ExitState();
		currentState = newState;
		currentState.EnterState();
	}

	void Update (){
		if (currentState != null){
			currentState.UpdateState();
		}
	}
}
