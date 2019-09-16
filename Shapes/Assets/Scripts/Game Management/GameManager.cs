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

	private AudioSource audioSource;

	public event Action GamePaused;
	public event Action UnpausedGame;
	//public event Action ExitedLevel;
	
	public bool PausedGame { get; set; }
	[SerializeField][Range(0.1f, 1f)]
	private float slowMotionSpeed = 0.3f;
	private const float ACTION_SHOT_PERCENTAGE_CHANCE = 5f;
	[SerializeField][Range(0, 100f)]
	private float actionShotPercentageChance = 30f;
	private const float FIXED_TIMESTEP = 0.01f;

	public static float deltaTime;
	private static float _lastframetime;

	// Check in Awake if there is an instance already, and if so, destroy the new instance.
	private void Awake()
	{
		GameSettings();
		audioSource = GetComponent<AudioSource>();
	}

	private void OnEnable()
	{
		SceneController.LoadedScene += StartFresh;
	}

	private void OnDisable()
	{
		SceneController.LoadedScene -= StartFresh;
	}

	private void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	private void StartFresh()
	{
		PausedGame = false;
		AudioListener.pause = false;
		EnableSlowMotion(false);
	}

	// Update is called once per frame
	void Update()
	{
		if(GameData.LevelIsActive)
		{
			GameData.IncrementTimePlayed();
			audioSource.enabled = false;
		}
		else
		{
			audioSource.enabled = true;
		}

		if(Input.GetKeyDown("p") && GameData.LevelIsActive)
		{
			PauseGame();
		}
		//HandleSlowMotion();
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

	// Percentage chance that the action shot will activate when
	// an enemy dies. This increase when an enemy dies, but doesn't active.
	public IEnumerator EnableActionShot()
	{
		if(UnityEngine.Random.value <= (actionShotPercentageChance / 100))
		{
			actionShotPercentageChance = ACTION_SHOT_PERCENTAGE_CHANCE;
			EnableSlowMotion(true);
			UIManager.Instance.DisplayUI(UIManager.CanvasNames.ActionShot, true);
			yield return new WaitForSeconds(1);
			EnableSlowMotion(false);
			UIManager.Instance.DisplayUI(UIManager.CanvasNames.ActionShot, false);
		}
		else
		{
			actionShotPercentageChance++;
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
			if(UnpausedGame != null)
			{
				UnpausedGame();
			}
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

	/*public void ReturnToLevelSelectMenu()
	{
		SceneController.Instance.LoadScene("LevelSelect");
		//_pausedGame = false;
		if(ExitedLevel != null)
		{
			ExitedLevel();
		}
	}*/

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
