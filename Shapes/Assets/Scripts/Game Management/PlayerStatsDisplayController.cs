using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class PlayerStatsDisplayController : MonoBehaviour
{
	// GameObjects
	public GameObject statBoxPrefab;
	public GameObject canvasParent;

	private StatBox[] statBoxes;

	// Start is called before the first frame update
	void Start()
	{
		InstantiateStatBoxes();
		InitializeStatBoxes();
	}

	// Update is called once per frame
	void Update()
	{

	}

	private void InstantiateStatBoxes()
	{
		for(int i = 0; i < GameData.playerStatsData.playerStats.Count; i++)
		{
			if(statBoxPrefab != null || canvasParent != null)
			{
				GameObject statBox = Instantiate(statBoxPrefab);
				statBox.transform.SetParent(canvasParent.transform, false);
			}
			else
			{
				Debug.LogError("Error: Couldn't find button prefab or it's parent canvas.");
			}
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
				statBoxes[i].Name = GameData.playerStatsData.playerStats[i].displayName;
				statBoxes[i].Value = GameData.playerStatsData.playerStats[i].value;
			}
		}
	}
}
