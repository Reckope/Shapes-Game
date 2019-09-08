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
		SlowMotion,
		PlayerDiedFilter,
		PlayerDiedButtons
	}

	public List<CanvasInfo> canvases = new List<CanvasInfo>();

	private void OnEnable()
	{
		LevelCompleteTrigger.CompletedLevel += DisplayCompletedLevelUI;
		GameManager.Instance.GamePaused += DisplayPausedGameUI;
		GameManager.Instance.UnpausedGame += HidePausedGameUI;
		GameManager.Instance.ActivateActionShot += DisplayActionShotUI;
		GameManager.Instance.DeactivateActionShot += HideActionShotUI;
	}

	private void OnDisable()
	{
		LevelCompleteTrigger.CompletedLevel -= DisplayCompletedLevelUI;
		GameManager.Instance.GamePaused -= DisplayPausedGameUI;
		GameManager.Instance.UnpausedGame -= HidePausedGameUI;
		GameManager.Instance.ActivateActionShot -= DisplayActionShotUI;
		GameManager.Instance.DeactivateActionShot -= HideActionShotUI;
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

	private void DisplayCompletedLevelUI(int level, bool successfulyCompleted)
	{

	}

	public void DisplayUI(CanvasNames canvasName, bool display)
	{
		CanvasInfo ui = canvases.Find((x) => x.name == canvasName);
		ui.canvas.SetActive(display);
	}

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
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.SlowMotion);
		ui.canvas.SetActive(true);
	}

	private void HideActionShotUI()
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.SlowMotion);
		ui.canvas.SetActive(false);
	}
}
