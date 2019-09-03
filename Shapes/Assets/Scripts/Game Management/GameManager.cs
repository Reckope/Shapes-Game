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

	public event Action PausedGame;
	public event Action UnpausedGame;
	public event Action ActivateActionShot;
	public event Action DeactivateActionShot;
	
	private bool _pausedGame = false;
	private bool _actionShotIsActive = false;
	[SerializeField][Range(0.1f, 1f)]
	private float slowMotionSpeed = 0.3f;
	private const float FIXED_TIMESTEP = 0.01f;

	// Check in Awake if there is an instance already, and if so, destroy the new instance.
	private void Awake()
	{
		GameSettings();
	}

	// Update is called once per frame
	void Update()
	{
		HandlePauseMenu();
		HandleSlowMotion();
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
				Time.timeScale = slowMotionSpeed;
				Time.fixedDeltaTime = FIXED_TIMESTEP * Time.timeScale;
				return;
			}
			else if(_actionShotIsActive)
			{
				if(DeactivateActionShot != null)
				{
					DeactivateActionShot();
				}
				_actionShotIsActive = false;
				Time.timeScale = 1;
    			Time.fixedDeltaTime = FIXED_TIMESTEP ;
				return;
			}
		}
	}

	private void HandlePauseMenu()
	{
		if(Input.GetKeyDown("p"))
		{
			if(!_pausedGame)
			{
				if(PausedGame != null)
				{
					PausedGame();
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

	public void ExitGame()
	{
		Application.Quit();
	}
}
