using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCompleteTrigger : MonoBehaviour
{
	Ped ped;

	public static event Action<int, bool> CompletedLevel;
	// Used for other objects (Camera, player etc)
	public static event Action LevelIsComplete;

	void Start()
	{
		//ped = GameObject.FindObjectOfType(typeof(Ped)) as Ped;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer(Ped.PedType.Player.ToString()))
		{
			AudioListener.pause = true;
			if(CompletedLevel != null)
			{
				CompletedLevel(GameData.ActiveLevelIndex, true);
			}
			if(LevelIsComplete != null)
			{
				LevelIsComplete();
			}
		}
	}
}
