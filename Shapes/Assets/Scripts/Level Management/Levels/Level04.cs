/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is the mission controller for Level04.
* Attach this to an empty object in Level04 scene.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Level04 : MonoBehaviour
{
	// Events
	public static event Action PlayLevel04Intro;

	// Global Variables
	private float actionShotInitialChance = 100f;
	[SerializeField]
	private float cameraIntroXAxis = 48f;
	[SerializeField]
	private float cameraIntroYAxis = 3.7f;

	private void Start()
	{
		if(PlayLevel04Intro != null)
		{
			PlayLevel04Intro();
		}

		Assert.AreEqual(actionShotInitialChance, 100f);
		GameManager.Instance.ActionShotPercentageChance = actionShotInitialChance;
		PlayScene();
	}

	private void PlayScene()
	{
		CameraController.Instance.SetCameraPosition(cameraIntroXAxis, cameraIntroYAxis);
		CameraController.Instance.PlayCutscene(CameraController.Cutscenes.Level04Intro);
	}
}
