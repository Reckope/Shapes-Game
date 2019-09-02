/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to coordinate actions across the entire game.
* Design Pattern implemented: Singleton
* Remove Scene clones - Yes. Global Access - Yes.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get { return _instance; } }
	private static GameManager _instance;

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
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log("Active Level: " + activeLevel);
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
