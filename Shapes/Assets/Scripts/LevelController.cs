/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to load and manage the levels. Data can be loaded from a Json file
* and buttons are instantiated to gain access to the levels. 
* Attach this to an empty gameobject. 
*/

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Structure of a Level.
[Serializable]
public struct LevelStructure
{
	public string levelID;
	public string levelName;
	public string description;
	public int buildIndex;
	public bool isUnlocked;
	public bool isActive;
	public bool completed;
}

// Data collection for the level. 
[Serializable]
public class LevelDataCollection
{
	public LevelStructure[] levels;
}

public class LevelController : MonoBehaviour
{
	// Classes
	LevelDataCollection allLevels;

	// GameObjects
	public GameObject buttonPrefab;
	public GameObject canvasParent;

	// Global Variables
	private Level[] levelButtons;
	private int numberOfLevels;


	// ------------------------------------------------------------------------------
	void Awake()
	{
		LoadLevelDataFromJsonFile();
		InstantiateLevelButtons();
		InitializeLevels();
	}

	// First we load the level data from an external Json file "LevelData". 
	private void LoadLevelDataFromJsonFile()
	{
		TextAsset txtAsset = (TextAsset)Resources.Load("LevelData", typeof(TextAsset));
		String levelData = txtAsset.text;
		allLevels = JsonUtility.FromJson<LevelDataCollection>(levelData);
	}

	// We then instantiate level buttons for each level in the json file. Designers can simply
	// add a level to the file, and this will automatically create a button for it. 
	private void InstantiateLevelButtons()
	{
		numberOfLevels = allLevels.levels.Length;
		Debug.Log(numberOfLevels);
		for(int i = 0; i < numberOfLevels; i++)
		{
			if(buttonPrefab != null || canvasParent != null)
			{
				GameObject levelButton = Instantiate(buttonPrefab);
				levelButton.transform.SetParent(canvasParent.transform, false);
			}
			else
			{
				Debug.LogError("Error: Couldn't find button prefab or it's parent canvas.");
			}
		}
		levelButtons = canvasParent.GetComponentsInChildren<Level>();
	}

	// After obtaining the level data, we construct each level in Level.cs.
	// Each button will contain the data for the level it activates. 
	private void InitializeLevels()
	{
		for(int i = 0; i < levelButtons.Length; i++)
		{
			levelButtons[i].gameObject.SetActive(true);
			levelButtons[i].ConstructLevel(
				allLevels.levels[i].levelID, 
				allLevels.levels[i].levelName,
				allLevels.levels[i].description,
				allLevels.levels[i].buildIndex,
				allLevels.levels[i].isUnlocked,
				allLevels.levels[i].isActive,
				allLevels.levels[i].completed
			);
		}
	}

	// This is called from the button 'Reset Level Data'. 
	public void ResetLevelData()
	{
		for(int i = 1; i < levelButtons.Length; i++)
		{
			allLevels.levels[i].isUnlocked = false;
			levelButtons[i].DisableLevel(allLevels.levels[i].isUnlocked);
		}
	}

	public int GetNumberOfLevels()
	{
		return numberOfLevels;
	}
}