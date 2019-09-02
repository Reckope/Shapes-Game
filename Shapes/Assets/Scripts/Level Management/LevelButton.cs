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
using UnityEngine.EventSystems;

public class LevelButton : MonoBehaviour
{
	// Leve Button Components
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
	// MonoBehaviour Methods.
	// ============================================================

	private void Awake()
	{
		GetButtonComponents();
	}

	void Start()
	{
		SetLevelUnlockState();
		//levelInformation.text = null;
	}

	// ============================================================
	// Level Button Methods
	// ============================================================

	private void GetButtonComponents()
	{
		if(GetComponent<Button>() == null)
		{
			levelButton = gameObject.AddComponent(typeof(Button)) as Button;
		}
		if(GetComponent<Image>() == null)
		{
			levelImage = gameObject.AddComponent(typeof(Image)) as Image;
		}
		levelButton = GetComponent<Button>();
		levelImage = GetComponent<Image>();
	}

	// We then enable the level depending on if it's unlocked or not. 
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
/*
	// Display the level information whilst the player hovers over the 
	// level button. 
	public void DisplayLevelInformation()
	{
		if(levelInformation != null)
		{
			if(this.isUnlocked)
			{
				levelInformation.text = description;
			}
			else
			{
				levelInformation.text = "LOCKED";
			}
		}
		else
		{
			Debug.Log("Missing Object Reference: Text levelInformation");
		}
	}
*/
	public void HideLevelInformation()
	{
		//levelInformation.text = null;
	}

	public void StartLevel()
	{
		if(Application.CanStreamedLevelBeLoaded(ID))
		{
			SceneManager.LoadScene(ID);
			LevelManager.Instance.SetActiveLevel(this.BuildIndex);
		}
		else
		{
			Debug.LogError("ERROR: Scene " + " '" + ID + "' " + " does not exist. Please check the spelling is correct, or if the scene needs adding to the build via File > Build Settings.");
		}
	}

	public void DisableLevel(bool unlocked)
	{
		IsUnlocked = unlocked;
		SetLevelUnlockState();
	}
}