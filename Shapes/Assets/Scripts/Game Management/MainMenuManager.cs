using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
	public GameObject mainMenuButtons;
	public GameObject confirmNewGamePrompt;
	public Button loadGameButton;

	// Start is called before the first frame update
	void Start()
	{
		HandleLoadGameButton();
		confirmNewGamePrompt.SetActive(false);
		mainMenuButtons.SetActive(true);
	}

	private void HandleLoadGameButton()
	{
		if(File.Exists(Application.dataPath + "/LevelData.json"))
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
