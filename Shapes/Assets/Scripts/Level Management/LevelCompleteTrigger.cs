/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to trigger the end of a level.
* Attach this to a trigger box collider. 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class LevelCompleteTrigger : MonoBehaviour
{
	// Events
	public static event Action<int, bool> CompletedLevel;
	// Used for other objects (Camera, player etc)
	public static event Action LevelIsComplete;

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
