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

public class Level : MonoBehaviour
{
	// Leve Button Components
	public Sprite lockedLevelSprite;
	public Text levelNumberText;
	public Text levelNameText;
	private Button levelButton;
	private Image levelImage;

	// Global Variables
	private string levelID { get; set; }
	private string levelName { get; set; }
	private string levelDescription { get; set; }
	private int buildIndex { get; set; }
	private bool isUnlocked { get; set; }
	private bool isActive { get; set; }
	private bool isCompleted { get; set; }

	// ------------------------------------------------------------------------------
	// During Awake(), the LevelController.cs Initializes each level, and constructs it here.  
	public void ConstructLevel(string _id, string _name, string _description, int _buildIndex, bool _isUnlocked, bool _isActive, bool _completed)
	{
		levelID = _id;
		name = _name;
		levelDescription = _description;
		buildIndex = _buildIndex;
		isUnlocked = _isUnlocked;
		isActive = _isActive;
		isCompleted = _completed;
	}

	void Start()
	{
		GetButtonComponents();
		SetLevelUnlockState();
		//levelInformation.text = null;
	}

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
		if(isUnlocked)
		{
			levelNumberText.text = buildIndex.ToString();
			levelNameText.text = name;
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
		if(Application.CanStreamedLevelBeLoaded(levelID))
		{
			SceneManager.LoadScene(levelID);
		}
		else
		{
			Debug.LogError("ERROR: Scene " + " '" + levelID + "' " + " does not exist. Please check the spelling is correct, or if the scene needs adding to the build via File > Build Settings.");
		}
	}

	public void DisableLevel(bool unlocked)
	{
		isUnlocked = unlocked;
		SetLevelUnlockState();
	}
}