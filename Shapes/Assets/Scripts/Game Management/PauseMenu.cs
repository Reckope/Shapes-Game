using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public void ReturnToLevelSelectMenu()
	{
		GameManager.Instance.ReturnToLevelSelectMenu();
	}

	public void UnPauseGame()
	{
		GameManager.Instance.PauseGame();
	}

	public void ExitGame()
	{
		GameManager.Instance.ConfirmExitGame();
	}

	public void HideExitGamePrompt()
	{
		GameManager.Instance.HideExitGamePrompt();
	}
}
