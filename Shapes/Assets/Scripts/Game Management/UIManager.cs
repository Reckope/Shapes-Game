using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct CanvasInfo
{
	public string name;
	public GameObject canvas;
}

public class UIManager : MonoBehaviour
{
	// Singleton
	public static UIManager Instance { get { return _instance; } }
	private static UIManager _instance;

	public List<CanvasInfo> canvases = new List<CanvasInfo>();

	private void OnEnable()
	{
		LevelCompleteTrigger.CompletedLevel += DisplayCompletedLevelUI;
		GameManager.Instance.PausedGame += DisplayPausedGameUI;
		GameManager.Instance.UnpausedGame += HidePausedGameUI;
		GameManager.Instance.ActivateActionShot += DisplayActionShotUI;
		GameManager.Instance.DeactivateActionShot += HideActionShotUI;
	}

	private void OnDisable()
	{
		LevelCompleteTrigger.CompletedLevel -= DisplayCompletedLevelUI;
		GameManager.Instance.PausedGame -= DisplayPausedGameUI;
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

	private void DisplayPausedGameUI()
	{
		canvases[0].canvas.SetActive(true);
	}

	private void HidePausedGameUI()
	{
		canvases[0].canvas.SetActive(false);
	}

	private void DisplayActionShotUI()
	{
		canvases[1].canvas.SetActive(true);
	}

	private void HideActionShotUI()
	{
		canvases[1].canvas.SetActive(false);
	}
}
