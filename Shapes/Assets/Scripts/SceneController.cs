﻿/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to navigate between scenes. Attach this to an ampty Scene Manager GameObject.
* Instead of using strings, I was trying to display an Enum paramater in the OnClick field within the inspector,
* but it turns out Unity doesn't support that - "https://answers.unity.com/questions/1549639/enum-as-a-function-param-in-a-button-onclick.html"
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class SceneController : MonoBehaviour
{

	// Check if the scene can be loaded.
	public void LoadScene(string sceneName)
	{
		if(Application.CanStreamedLevelBeLoaded(sceneName))
		{
			SceneManager.LoadScene(sceneName);
		}
		else
		{
			Debug.LogError("ERROR: Scene " + " '" + sceneName + "' " + " does not exist. Please check the spelling is correct, or if the scene needs adding to the build via File > Build Settings.");
		}
	}

	public static String GetActiveScene()
	{
		return SceneManager.GetActiveScene().name;
	}
}