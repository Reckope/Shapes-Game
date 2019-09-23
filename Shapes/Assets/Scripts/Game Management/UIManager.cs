/* Author: Joe Davis
 * Project: Shapes
 * 2019
 * Notes:
 * This controls & stores the UI throughout the game.
 * Attach this to the canvas within the scene.

 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

// Information about the canvas beign loaded. 
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

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void OnEnable()
	{
		LevelCompleteTrigger.LevelIsComplete += DisplayCompletedLevelUI;
	}

	private void Start()
	{
		Instanced();
	}

	private void OnDisable()
	{
		LevelCompleteTrigger.LevelIsComplete -= DisplayCompletedLevelUI;
	}

	// =========================================================
	// UI Controller Methods.
	// =========================================================

	private void Instanced()
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
		Assert.IsNotNull(ui);
		ui.canvas.SetActive(display);
	}

	// This is used by events that dont pass over a canvas name. 
	private void DisplayCompletedLevelUI()
	{
		CanvasInfo ui = canvases.Find((x) => x.name == CanvasNames.CompletedLevel);
		Assert.IsNotNull(ui);
		ui.canvas.SetActive(true);
	}
}
