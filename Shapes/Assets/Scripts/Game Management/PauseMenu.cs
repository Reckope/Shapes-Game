/* Author: Joe Davis
 * Project: Shapes
 * 2019
 * Notes:
 * This contains methods for buttons within the pause menu to call.
 * Attach this to the scenes canvas. 
 */

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
		Application.Quit();
	}

	public void ConfirmExitGame()
	{
		GameManager.Instance.DisplayExitGamePrompt();
	}

	public void HideExitGamePrompt()
	{
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.ExitGamePrompt, false);
	}
}
