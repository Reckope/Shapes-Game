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


public class LevelManager : MonoBehaviour
{
	// Singleton
	public static LevelManager Instance { get { return _instance; } }
	private static LevelManager _instance;

	// Classes
	//public LevelDataCollection levelData;

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
		InstantiateLevelButtons();
		InitializeLevels();
		Debug.Log(Application.dataPath);
	}

	// ============================================================
	// Level Manager Methods
	// ============================================================

	// We then instantiate level buttons for each level in the json file. Designers can simply
	// add a level to the file, and this will automatically create a button for it. 
	private void InstantiateLevelButtons()
	{
		for(int i = 0; i < GameData.levelData.levels.Count; i++)
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
				levelButtons[i].ID = GameData.levelData.levels[i].levelID;
				levelButtons[i].LevelName = GameData.levelData.levels[i].levelName;
				levelButtons[i].Description = GameData.levelData.levels[i].description;
				levelButtons[i].BuildIndex = GameData.levelData.levels[i].buildIndex;
				levelButtons[i].IsUnlocked = GameData.levelData.levels[i].isUnlocked;
				levelButtons[i].IsActive = GameData.levelData.levels[i].isActive;
				levelButtons[i].IsCompleted = GameData.levelData.levels[i].isCompleted;
			}
		}
	}

	// ============================================================
	// Methods other classes / buttons can call.
	// ============================================================

	// This is called from the button 'Reset Level Data'. 
	public void ResetLevelData()
	{
		for(int i = 2; i < levelButtons.Length; i++)
		{
			GameData.levelData.levels[i].isUnlocked = false;
			levelButtons[i].DisableLevel(GameData.levelData.levels[i].isUnlocked);
		}
	}

	public void SetActiveLevel(int index, bool activeOrNot)
	{
		// Use a Lambda expression to find and set the level info :)
		LevelInfo level = GameData.levelData.levels.Find((x) => x.buildIndex == index);

		level.isActive = activeOrNot;

		if(level.isActive)
		{
			GameData.ActiveLevelName = level.levelName;
			GameData.ActiveLevelIndex = level.buildIndex;
			GameData.LevelIsActive = true;
		}
		else
		{
			GameData.ActiveLevelName = "Not currently on a level.";
			GameData.ActiveLevelIndex = -1;
			GameData.LevelIsActive = false;
		}

		Debug.Log("Active Level: " + level.levelName + " Is Active: " + level.isActive);
	}

	private void CompleteLevel(int completedLevelBuildIndex, bool successfullyCompleted)
	{
		LevelInfo level = GameData.levelData.levels.Find((x) => x.buildIndex == completedLevelBuildIndex);

		level.isActive = false;
		GameData.LevelIsActive = false;
		Debug.Log(level.buildIndex);
		if(successfullyCompleted)
		{
			level.isCompleted = true;
			if(completedLevelBuildIndex == HighestUnlockedLevelBuildIndex())
			{
				GameData.ActiveLevelIndex++;
			}
			GameData.levelData.levels[GameData.ActiveLevelIndex].isUnlocked = true;
		}
		SceneManager.LoadScene("LevelSelect");

		Debug.Log("Active Level: " + level.levelName + " Is Active: " + level.isActive);
	}

	private int HighestUnlockedLevelBuildIndex()
	{
		int numberOfUnlockedLevels = 0;

		foreach(LevelInfo level in GameData.levelData.levels)
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
		if(GameData.LevelIsActive)
		{
			SetActiveLevel(GameData.ActiveLevelIndex, false);
		}
	}
}