/* Author: Joe Davis
 * Project: Shapes
 * 2019
 * Notes:
 * This is used on the main menu, where the player initially loads into. 
 * Attach this to an object in MainMenu.
 */

using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class MainMenuManager : MonoBehaviour
{
	// GameObjects
	public GameObject mainMenuButtons;
	public GameObject confirmNewGamePrompt;
	public Button loadGameButton;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Start()
	{
		Assert.IsNotNull(loadGameButton);
		InteractWithLoadGameButton();

		Assert.IsNotNull(confirmNewGamePrompt);
		confirmNewGamePrompt.SetActive(false);

		Assert.IsNotNull(mainMenuButtons);
		mainMenuButtons.SetActive(true);
	}

	// =========================================================
	// Main Menu Methods
	// =========================================================

	private void InteractWithLoadGameButton()
	{
		if(File.Exists(Application.dataPath + GameData.LevelDataFileName))
		{
			loadGameButton.interactable = true;
		}
		else
		{
			loadGameButton.interactable = false;
		}
	}

	public void ActivateNewGameButton()
	{
		if(File.Exists(GameData.LevelFilePathLocation))
		{
			confirmNewGamePrompt.SetActive(true);
			mainMenuButtons.SetActive(false);
		}
		else
		{
			StartNewGame();
		}
	}

	public void StartNewGame()
	{
		if(File.Exists(GameData.LevelFilePathLocation))
		{
			GameData.DeleteExistingSave();
		}
		GameData.LoadLevelDataFromResources();
		GameData.SaveGame();
		SceneController.Instance.LoadScene("InGameMenu");
	}

	// =========================================================
	// These methods are used by the buttons in the Main Menu.
	// =========================================================

	public void LoadExistingGame()
	{
		GameData.LoadGame();
		SceneController.Instance.LoadScene("InGameMenu");
	}

	public void HideConfirmNewGamePrompt()
	{
		confirmNewGamePrompt.SetActive(false);
		mainMenuButtons.SetActive(true);
	}
}
