using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CanvasInfo
{
	public UIManager.CanvasNames name;
	public GameObject canvas;
}

public class UIManager : MonoBehaviour
{
	// Singleton
	public static UIManager Instance { get { return _instance; } }
	private static UIManager _instance;

	public enum CanvasNames
	{
		PauseMenu,
		ExitGamePrompt,
		MainMenuPrompt,
		ActionShot,
		PlayerDiedFilter,
		PlayerDiedButtons,
		CompletedLevel
	}

	public List<CanvasInfo> canvases = new List<CanvasInfo>();

	private void OnEnable()
	{
		LevelCompleteTrigger.CompletedLevel += DisplayCompletedLevelUI;
	}

	private void OnDisable()
	{
		LevelCompleteTrigger.CompletedLevel -= DisplayCompletedLevelUI;
	}

	// Start is called before the first frame update
	void Start()
	{
		if(_instance != null && _instance != this)
		{
			Debug.LogError("Error: Another instance of UIManager has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}
	}

	public void DisplayUI(CanvasNames canvasName, bool display)
	{
		CanvasInfo ui = canvases.Find((x) => x.name == canvasName);
		ui.canvas.SetActive(display);
	}

	private void DisplayCompletedLevelUI(int level, bool successfulyCompleted)
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.CompletedLevel);
		ui.canvas.SetActive(true);
	}
/* 
	private void DisplayPausedGameUI()
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.PauseMenu);
		ui.canvas.SetActive(true);
	}

	private void HidePausedGameUI()
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.PauseMenu);
		ui.canvas.SetActive(false);
	}

	private void DisplayActionShotUI()
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.ActionShot);
		ui.canvas.SetActive(true);
	}

	private void HideActionShotUI()
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.ActionShot);
		ui.canvas.SetActive(false);
	}*/
}
