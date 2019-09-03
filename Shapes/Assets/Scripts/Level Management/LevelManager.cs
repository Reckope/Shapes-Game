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
using UnityEngine.SceneManagement;

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
	private LevelButton[] levelButtons;

	// ============================================================
	// MonoBehaviour Methods
	// ============================================================

	private void OnEnable()
	{
		LevelCompleteTrigger.CompletedLevel += CompleteLevel;
		SceneController.LoadedScene += HandleExitLevel;
	}

	private void Awake()
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
	}

	private void Start()
	{
		if(GlobalLevelData.levels.Count == 0)
		{
			LoadLevelDataFromJsonFile();
		}
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
		GlobalLevelData.levels = new List<LevelInfo>(levelData.levels);
	}

	// We then instantiate level buttons for each level in the json file. Designers can simply
	// add a level to the file, and this will automatically create a button for it. 
	private void InstantiateLevelButtons()
	{
		for(int i = 0; i < GlobalLevelData.levels.Count; i++)
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

	// After obtaining the level data, we pass over each levels data to it's given button.
	// Each button will contain the data for the level it activates. 
	private void InitializeLevels()
	{
		for(int i = 0; i < levelButtons.Length; i++)
		{
			if(levelButtons[i] != null)
			{
				levelButtons[i].gameObject.SetActive(true);
				levelButtons[i].ID = GlobalLevelData.levels[i].levelID;
				levelButtons[i].LevelName = GlobalLevelData.levels[i].levelName;
				levelButtons[i].Description = GlobalLevelData.levels[i].description;
				levelButtons[i].BuildIndex = GlobalLevelData.levels[i].buildIndex;
				levelButtons[i].IsUnlocked = GlobalLevelData.levels[i].isUnlocked;
				levelButtons[i].IsActive = GlobalLevelData.levels[i].isActive;
				levelButtons[i].IsCompleted = GlobalLevelData.levels[i].isCompleted;
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
			GlobalLevelData.levels[i].isUnlocked = false;
			levelButtons[i].DisableLevel(GlobalLevelData.levels[i].isUnlocked);
		}
	}

	public void SetActiveLevel(int index, bool activeOrNot)
	{
		// Use a Lambda expression to find and set the level info :)
		LevelInfo level = GlobalLevelData.levels.Find((x) => x.buildIndex == index);

		level.isActive = activeOrNot;

		if(level.isActive)
		{
			GlobalLevelData.ActiveLevelName = level.levelName;
			GlobalLevelData.ActiveLevelIndex = level.buildIndex;
			GlobalLevelData.LevelIsActive = true;
		}
		else
		{
			GlobalLevelData.ActiveLevelName = "Not currently on a level.";
			GlobalLevelData.ActiveLevelIndex = -1;
			GlobalLevelData.LevelIsActive = false;
		}

		Debug.Log("Active Level: " + level.levelName + " Is Active: " + level.isActive);
	}

	private void CompleteLevel(int completedLevelBuildIndex, bool successfullyCompleted)
	{
		LevelInfo level = GlobalLevelData.levels.Find((x) => x.buildIndex == completedLevelBuildIndex);

		level.isActive = false;
		if(successfullyCompleted)
		{
			level.isCompleted = true;
			if(completedLevelBuildIndex == HighestUnlockedLevelBuildIndex())
			{
				GlobalLevelData.ActiveLevelIndex++;
			}
			GlobalLevelData.levels[GlobalLevelData.ActiveLevelIndex].isUnlocked = true;
		}
		SceneManager.LoadScene("LevelSelect");

		Debug.Log("Active Level: " + level.levelName + " Is Active: " + level.isActive);
	}

	private int HighestUnlockedLevelBuildIndex()
	{
		int numberOfUnlockedLevels = 0;

		foreach(LevelInfo level in GlobalLevelData.levels)
		{
			if(level.isUnlocked && level.buildIndex != 0)
			{
				numberOfUnlockedLevels++;
			}
		}
		return numberOfUnlockedLevels;
	}

	private void HandleExitLevel()
	{
		if(GlobalLevelData.LevelIsActive)
		{
			SetActiveLevel(GlobalLevelData.ActiveLevelIndex, false);
		}
	}
}