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

// Use a class instead of a structure, as values will be changed in the list. 
[Serializable]
public class LevelInfo
{
	public string levelID;
	public string levelName;
	public string description;
	public int buildIndex;
	public bool isUnlocked;
	public bool isActive;
	public bool isCompleted;
}

// Store the levels from LevelData.json in a generic list.
[Serializable]
public class LevelDataCollection
{
	public List<LevelInfo> levels = new List<LevelInfo>();
}

public class LevelManager : MonoBehaviour
{
	// Singleton
	public static LevelManager Instance { get { return _instance; } }
	private static LevelManager _instance;

	// Classes
	public LevelDataCollection levelData;

	// GameObjects
	public GameObject buttonPrefab;
	public GameObject canvasParent;

	// Global Variables
	public string ActiveLevelName { get; set; }
	public int NumberOfLevels { get; private set; }
	private LevelButton[] levelButtons;


	// ============================================================
	// MonoBehaviour Methods
	// ============================================================

	void Awake()
	{
		if(_instance != null && _instance != this)
		{
			Debug.LogError("Error: Another instance of LevelManager has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}
		LoadLevelDataFromJsonFile();
		InstantiateLevelButtons();
		InitializeLevels();
	}

	// ============================================================
	// Level Manager Methods
	// ============================================================

	// First we load the level data from an external Json file "LevelData". 
	private void LoadLevelDataFromJsonFile()
	{
		TextAsset textAsset = (TextAsset)Resources.Load("LevelData", typeof(TextAsset));
		String data = textAsset.text;
		levelData = JsonUtility.FromJson<LevelDataCollection>(data);
		NumberOfLevels = levelData.levels.Count;
		Debug.Log("List contains " + levelData.levels.Count + " entries.");
	}

	// We then instantiate level buttons for each level in the json file. Designers can simply
	// add a level to the file, and this will automatically create a button for it. 
	private void InstantiateLevelButtons()
	{
		//numberOfLevels = levelData.levels.Length;
		Debug.Log("Total Number of Levels: " + NumberOfLevels);
		for(int i = 0; i < NumberOfLevels; i++)
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
		levelButtons = canvasParent.GetComponentsInChildren<LevelButton>();
	}

	// After obtaining the level data, we pass over level data to an instance of Level.cs.
	// Each button will contain the data for the level it activates. 
	private void InitializeLevels()
	{
		for(int i = 0; i < levelButtons.Length; i++)
		{
			if(levelButtons[i] != null)
			{
				levelButtons[i].gameObject.SetActive(true);
				levelButtons[i].ID = levelData.levels[i].levelID;
				levelButtons[i].LevelName = levelData.levels[i].levelName;
				levelButtons[i].Description = levelData.levels[i].description;
				levelButtons[i].BuildIndex = levelData.levels[i].buildIndex;
				levelButtons[i].IsUnlocked = levelData.levels[i].isUnlocked;
				levelButtons[i].IsActive = levelData.levels[i].isActive;
				levelButtons[i].IsCompleted = levelData.levels[i].isCompleted;
			}
		}
	}

	// ============================================================
	// Methods other classes / buttons can call.
	// ============================================================

	// This is called from the button 'Reset Level Data'. 
	public void ResetLevelData()
	{
		for(int i = 1; i < levelButtons.Length; i++)
		{
			levelData.levels[i].isUnlocked = false;
			levelButtons[i].DisableLevel(levelData.levels[i].isUnlocked);
		}
	}

	public void SetActiveLevel(int levelBuildIndex)
	{
		levelData.levels[levelBuildIndex].isActive = true;
		ActiveLevelName = levelData.levels[levelBuildIndex].levelName;
		Debug.Log("Level: '" + levelData.levels[levelBuildIndex].levelName + "' is active: " + levelData.levels[levelBuildIndex].isActive);
	}
}