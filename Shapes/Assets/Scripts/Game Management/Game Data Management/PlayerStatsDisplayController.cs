/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to display the players' stats on the level manager scene.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerStatsDisplayController : MonoBehaviour
{
	// GameObjects
	public GameObject statBoxPrefab;
	public GameObject canvasParent;

	private StatBox[] statBoxes;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	void Start()
	{
		InstantiateStatBoxes();
		InitializeStatBoxes();
	}

	// =========================================================
	// Stat Box Display Methods
	// =========================================================

	private void InstantiateStatBoxes()
	{
		Assert.IsNotNull(statBoxPrefab);
		Assert.IsNotNull(canvasParent);
		for(int i = 0; i < GameData.playerStatsData.playerStats.Count; i++)
		{
			GameObject statBox = Instantiate(statBoxPrefab);
			statBox.transform.SetParent(canvasParent.transform, false);
		}
		statBoxes = canvasParent.GetComponentsInChildren<StatBox>();
	}

	private void InitializeStatBoxes()
	{
		for(int i = 0; i < statBoxes.Length; i++)
		{
			if(statBoxes[i] != null)
			{
				statBoxes[i].gameObject.SetActive(true);
				statBoxes[i].ID = GameData.playerStatsData.playerStats[i].ID;
				statBoxes[i].Name = GameData.playerStatsData.playerStats[i].displayName;
				statBoxes[i].Value = GameData.playerStatsData.playerStats[i].value;
			}
		}
	}
}
