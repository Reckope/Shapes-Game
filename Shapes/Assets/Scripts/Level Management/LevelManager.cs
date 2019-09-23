/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to load and manage the levels. Data can be loaded from a Json file
* and buttons are instantiated to gain access to the levels. 
* Attach this to an empty gameobject in the LevelSelect scene. 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class LevelManager : MonoBehaviour
{
	// Singleton
	public static LevelManager Instance { get { return _instance; } }
	private static LevelManager _instance;

	// GameObjects
	public GameObject buttonPrefab;
	public GameObject canvasParent;
	public Text feedbackText;

	// Global Variables
	private LevelButton[] levelButtons;

	//Events & variables
	public static event Action<bool> LevelHasActivated;
	public static bool LevelIsCurrentlyActive { get; set; }

	// ============================================================
	// MonoBehaviour Methods
	// ============================================================

	private void Awake()
	{
		Instanced();
	}

	private void OnEnable()
	{
		// Don't unsubscribe from these!! Otherwise, the next level
		// won't be unlocked.
		LevelCompleteTrigger.CompletedLevel += CompleteLevel;
		GameManager.ExitedLevel += HandleExitLevel;
	}

	private void Start()
	{
		InstantiateLevelButtons();
		InitializeLevels();
	}

	// ============================================================
	// Level Manager Methods
	// ============================================================

	private void Instanced()
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
		if(LevelHasActivated != null)
		{
			LevelHasActivated(activeOrNot);
		}
		// Use a Lambda expression to find and set the level info :)
		LevelInfo level = GameData.levelData.levels.Find((x) => x.buildIndex == index);
		Assert.IsNotNull(level);
		level.isActive = activeOrNot;

		if(level.isActive)
		{
			GameData.ActiveLevelName = level.levelName;
			GameData.ActiveLevelIndex = level.buildIndex;
			LevelIsCurrentlyActive = true;
		}
		else
		{
			GameData.ActiveLevelName = "Not currently on a level.";
			LevelIsCurrentlyActive = false;
		}

		Debug.Log("Active Level: " + level.levelName + " Is Active: " + level.isActive);
	}

	public void CompleteLevel(int completedLevelBuildIndex, bool successfullyCompleted)
	{
		Debug.Log("Completed level! " + completedLevelBuildIndex + " " + successfullyCompleted);
		LevelInfo level = GameData.levelData.levels.Find((x) => x.buildIndex == completedLevelBuildIndex);

		level.isActive = false;
		if(successfullyCompleted)
		{
			level.isCompleted = true;
			if(completedLevelBuildIndex == HighestUnlockedLevelBuildIndex() && completedLevelBuildIndex++ < GameData.levelData.levels.Count)
			{
				GameData.ActiveLevelIndex++;
				GameData.levelData.levels[GameData.ActiveLevelIndex].isUnlocked = true;
			}
		}
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

	// ============================================================
	// These methods are used for buttons in LevelSelect to call. 
	// ============================================================

	private void HandleExitLevel()
	{
		SetActiveLevel(GameData.ActiveLevelIndex, false);
	}

	public void SaveGame()
	{
		GameManager.Instance.SaveGame();
	}

	public void ReturnToPreviousMenu()
	{
		GameManager.Instance.SaveGame();
		SceneController.Instance.LoadScene("InGameMenu");
	}

	public void ConfirmExitGame()
	{
		GameManager.Instance.DisplayExitGamePrompt();
	}

	public void ExitGame()
	{
		GameManager.Instance.ExitGame();
	}

	public void HideExitGamePrompt()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.ExitGamePrompt, false);
	}

	public void DisplayFeedbackText(string feedback)
	{
		Assert.IsNotNull(feedbackText);
		feedbackText.text = feedback;
	}
}