/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to coordinate actions across the entire game.
* This persists across all scenes
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameManager : MonoBehaviour
{
	// Singleton
	public static GameManager Instance { get { return _instance; } }
	private static GameManager _instance;

	// Events
	public event Action GamePaused;
	public static event Action ExitedLevel;
	public event Action<bool> ActionShotIsActive;
	
	// Global variables
	public bool PausedGame { get; set; }
	public float ActionShotPercentageChance { get; set; }
	private const float INITIAL_ACTION_SHOT_PERCENTAGE_CHANCE = 5f;
	[SerializeField][Range(0, 100f)]
	private const float FIXED_TIMESTEP = 0.01f;
	[SerializeField][Range(0.1f, 1f)]
	private float slowMotionSpeed = 0.7f;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Awake()
	{
		GameSettings();
	}

	private void OnEnable()
	{
		SceneController.LoadedScene += DisablePauseAndSlowMotion;
		Ped.EnemyHasDied += ActionShot;
	}

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	private void Update()
	{
		if(LevelManager.LevelIsCurrentlyActive)
		{
			GameData.IncrementTimePlayed();
		}

		if(Input.GetKeyDown("p") && LevelManager.LevelIsCurrentlyActive)
		{
			PauseGame();
		}
	}

	// =========================================================
	// GameManager Methods
	// =========================================================

	private void OnDisable()
	{
		SceneController.LoadedScene -= DisablePauseAndSlowMotion;
		Ped.EnemyHasDied -= ActionShot;
	}

	private void DisablePauseAndSlowMotion()
	{
		PausedGame = false;
		EnableSlowMotion(false);
	}

	private void GameSettings()
	{
		if(_instance != null && _instance != this)
		{
			Debug.Log("Error: Another instance of GameManager has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}

		// Make the game run as fast as possible
		Application.targetFrameRate = 300;
	}

	// Wrapper for event 
	private void ActionShot()
	{
		StartCoroutine(EnableActionShot());
	}

	// Percentage chance that the action shot will activate when
	// an enemy dies. This increase when an enemy dies, but doesn't active.
	public IEnumerator EnableActionShot()
	{
		if(UnityEngine.Random.value <= (ActionShotPercentageChance / 100))
		{
			if(ActionShotIsActive != null)
			{
				ActionShotIsActive(true);
			}
			ActionShotPercentageChance = INITIAL_ACTION_SHOT_PERCENTAGE_CHANCE;
			EnableSlowMotion(true);
			UIManager.Instance.DisplayUI(UIManager.CanvasNames.ActionShot, true);
			yield return new WaitForSeconds(1);
			if(ActionShotIsActive != null)
			{
				ActionShotIsActive(false);
			}
			EnableSlowMotion(false);
			UIManager.Instance.DisplayUI(UIManager.CanvasNames.ActionShot, false);
		}
		else
		{
			ActionShotPercentageChance++;
		}
	}

	public void PauseGame()
	{
		if(!PausedGame)
		{
			if(GamePaused != null)
			{
				GamePaused();
			}
			PausedGame = true;
			AudioListener.pause = true;
			Time.timeScale = 0;
			Time.fixedDeltaTime = FIXED_TIMESTEP * Time.timeScale;
			UIManager.Instance.DisplayUI(UIManager.CanvasNames.PauseMenu, true);
			return;
		}
		else if(PausedGame)
		{
			PausedGame = false;
			AudioListener.pause = false;
			Time.timeScale = 1;
			Time.fixedDeltaTime = FIXED_TIMESTEP;
			UIManager.Instance.DisplayUI(UIManager.CanvasNames.PauseMenu, false);
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

	// =========================================================
	// These methods are called by various buttons in game. 
	// =========================================================

	public void DisplayExitGamePrompt()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.ExitGamePrompt, true);
	}

	public void HideMenuPrompt()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.MainMenuPrompt, false);
	}

	/*public void SaveBeforeReturningToPreviousMenu()
	{
		SaveGame();
		SceneController.Instance.LoadScene("InGameMenu");
	}*/

	public void ReturnToLevelSelectMenu()
	{
		LevelManager.LevelIsCurrentlyActive = false;
		SceneController.Instance.LoadScene("LevelSelect");
		if(ExitedLevel != null)
		{
			ExitedLevel();
		}
	}

	// Path for buttons
	public void SaveGame()
	{
		GameData.SaveGame();
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
