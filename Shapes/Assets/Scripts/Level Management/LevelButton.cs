/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to display the unlocked level and it's data, or disable it if not unlocked yet. 
* Attach this to a level UI button prefab. 
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class LevelButton : MonoBehaviour
{
	// Level Button Components
	public Sprite lockedLevelSprite;
	public Text levelNumberText;
	public Text levelNameText;
	private Button levelButton;
	private Image levelImage;

	// Global Variables
	public string ID { get; set; }
	public string LevelName { get; set; }
	public string Description { get; set; }
	public int BuildIndex { get; set; }
	public bool IsUnlocked { get; set; }
	public bool IsActive { get; set; }
	public bool IsCompleted { get; set; }

	// ============================================================
	// MonoBehaviour Methods (In order of execution)
	// ============================================================

	private void Awake()
	{
		levelButton = GetComponent<Button>();
		Assert.IsNotNull(levelButton);
		levelImage = GetComponent<Image>();
		Assert.IsNotNull(levelImage);

		Assert.IsNotNull(levelNumberText);
		Assert.IsNotNull(lockedLevelSprite);
		Assert.IsNotNull(levelNameText);
	}

	private void Start()
	{
		SetLevelUnlockState();
	}

	// ============================================================
	// Level Button Methods
	// ============================================================

	// We enable the level depending on if it's unlocked or not. 
	public void SetLevelUnlockState()
	{
		if(IsUnlocked)
		{
			levelNumberText.text = BuildIndex.ToString();
			levelNameText.text = LevelName;
			levelButton.interactable = true;
		}
		else
		{
			levelNameText.color = new Color(255, 255, 255);
			levelNumberText.text = null;
			levelNameText.text = "Locked";
			levelImage.sprite = lockedLevelSprite;
			levelButton.interactable = false;
		}
	}

	// WHen the level button is clicked
	public void StartLevel()
	{
		if(Application.CanStreamedLevelBeLoaded(ID))
		{
			LevelManager.Instance.SetActiveLevel(BuildIndex, true);
			SceneManager.LoadScene(ID);
		}
		else
		{
			Debug.LogError("ERROR: Scene " + " '" + ID + "' " + " does not exist. Please check the spelling is correct, or if the scene needs adding to the build via File > Build Settings.");
		}
	}

	// Called by the levelManager to update level locked status. 
	public void DisableLevel(bool unlocked)
	{
		IsUnlocked = unlocked;
		SetLevelUnlockState();
	}
}