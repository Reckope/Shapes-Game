using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public static event Action ExitedLevel;

	public void ReturnToLevelSelectMenu()
	{
		SceneController.Instance.LoadScene("LevelSelect");
		if(ExitedLevel != null)
		{
			ExitedLevel();
		}
	}

	public void UnPauseGame()
	{
		GameManager.Instance.PauseGame();
	}

	public void ExitGame()
	{
		GameManager.Instance.ConfirmExitGame();
	}
}
