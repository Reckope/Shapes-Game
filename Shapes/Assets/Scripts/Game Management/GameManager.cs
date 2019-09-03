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
	private bool _pausedGame = false;

	//public static string activeLevel;

	// Check in Awake if there is an instance already, and if so, destroy the new instance.
	private void Awake()
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

	// Update is called once per frame
	void Update()
	{
		//Debug.Log("Active Level: " + activeLevel);
		HandlePauseMenu();
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
				Time.timeScale = 0.3f;
				Time.fixedDeltaTime = 0.02F * Time.timeScale;
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
    			Time.fixedDeltaTime = 0.02F ;
				return;
			}
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
