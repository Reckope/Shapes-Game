/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to coordinate actions across the entire game.
* Design Pattern implemented: Singleton
* Remove Scene clones - Yes. Global Access - Yes.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	// Singleton
	public static GameManager Instance { get { return _instance; } }
	private static GameManager _instance;

	public event Action GamePaused;
	public event Action UnpausedGame;
	public event Action ExitedLevel;
	public event Action ActivateActionShot;
	public event Action DeactivateActionShot;
	
	private bool _pausedGame = false;
	private bool _actionShotIsActive = false;
	[SerializeField][Range(0.1f, 1f)]
	private float slowMotionSpeed = 0.3f;
	private const float FIXED_TIMESTEP = 0.01f;

	public static float deltaTime;
	private static float _lastframetime;

	// Check in Awake if there is an instance already, and if so, destroy the new instance.
	private void Awake()
	{
		GameSettings();
	}

	private void Start()
	{
		//SceneController.LoadedScene += PauseGame;
	}

	// Update is called once per frame
	void Update()
	{
		if(GameData.LevelIsActive)
		{
			GameData.IncrementTimePlayed();
		}
		if(Input.GetKeyDown("p"))
		{
			PauseGame();
		}
		HandleSlowMotion();
	}

	private void GameSettings()
	{
		if(_instance != null && _instance != this)
		{
			Debug.LogError("Error: Another instance of GameManager has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}

		// Make the game run as fast as possible
		Application.targetFrameRate = 300;
	}

	private void HandleSlowMotion()
	{
		if(Input.GetKeyDown("l"))
		{
			if(!_actionShotIsActive)
			{
				if(ActivateActionShot != null)
				{
					ActivateActionShot();
				}
				_actionShotIsActive = true;
				EnableSlowMotion(true);
				return;
			}
			else if(_actionShotIsActive)
			{
				if(DeactivateActionShot != null)
				{
					DeactivateActionShot();
				}
				_actionShotIsActive = false;
				EnableSlowMotion(false);
				return;
			}
		}
	}

	public void PauseGame()
	{
		if(!_pausedGame && GameData.LevelIsActive)
		{
			if(GamePaused != null)
			{
				GamePaused();
			}
			_pausedGame = true;
			Time.timeScale = 0;
			Time.fixedDeltaTime = FIXED_TIMESTEP * Time.timeScale;
			return;
		}
		else if(_pausedGame)
		{
			if(UnpausedGame != null)
			{
				UnpausedGame();
			}
			_pausedGame = false;
			Time.timeScale = 1;
			Time.fixedDeltaTime = FIXED_TIMESTEP;
			return;
		}
	}

	public void EnableSlowMotion(bool enabled)
	{
		if(enabled)
		{
			Time.timeScale = slowMotionSpeed;
			Time.fixedDeltaTime = FIXED_TIMESTEP * Time.timeScale;
		}
		else
		{
			Time.timeScale = 1;
    		Time.fixedDeltaTime = FIXED_TIMESTEP ;
		}
	}

	public void ReturnToLevelSelectMenu()
	{
		SceneController.Instance.LoadScene("LevelSelect");
		//_pausedGame = false;
		if(ExitedLevel != null)
		{
			ExitedLevel();
		}
	}

	public void ConfirmExitGame()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.ExitGamePrompt, true);
	}

	public void HidePrompt()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.ExitGamePrompt, false);
	}

	public void HideMenuPrompt()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.MainMenuPrompt, false);
	}

	public void SaveBeforeReturningToMainMenu()
	{
		Savegame();
		SceneController.Instance.LoadScene("MainMenu");
	}

	// Path for buttons
	public void Savegame()
	{
		GameData.Savegame();
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
