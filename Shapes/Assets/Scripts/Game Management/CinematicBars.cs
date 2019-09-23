/* Author: Joe Davis
 * Project: Shapes
 * 2019
 * Notes:
 * Black bars are created via this script instead of using game objects. This
 * can be useful for future projects :) All you need to do is call ShowCinematicBars or HideCinematicBars.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class CinematicBars : MonoBehaviour 
{
	// Bars
	private RectTransform topBar, bottomBar;

	// Global Variables
	private const int ANCHOR_MIN = 0;
	private const int ANCHOR_MAX = 1;

	private float barSize = 300f;
	private float changeSizeAmount;
	private float targetSize;
	private float barSpeed;
	private bool isActive;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	void OnEnable()
	{
		CreateBars();
	}
	
	// Update is called once per frame
	private void Update () 
	{
		if(isActive && !Player.Instance.isDead)
		{
			Vector2 sizeDelta = topBar.sizeDelta;
			sizeDelta.y += changeSizeAmount * Time.deltaTime;
			if(changeSizeAmount > ANCHOR_MIN)
			{
				if(sizeDelta.y >= targetSize)
				{
					sizeDelta.y = targetSize;
					isActive = false;
				}
			}
			else
			{
				if(sizeDelta.y <= targetSize)
				{
					sizeDelta.y = targetSize;
					isActive = false;
				}
			}
			topBar.sizeDelta = sizeDelta;
			bottomBar.sizeDelta = sizeDelta;
		}
	}

	// =========================================================
	// Cinematic Bars Methods
	// =========================================================

	// Display the cinematic bars
	public void ShowCinematicBars(float barSpeed)
	{
		float targetSize = barSize;
		this.targetSize = targetSize;
		changeSizeAmount = (targetSize - topBar.sizeDelta.y) / barSpeed;
		isActive = true;
	}

	// Hide the cinematic bars
	public void HideCinematicBars(float barSpeed)
	{
		targetSize = ANCHOR_MIN;
		changeSizeAmount = (targetSize - topBar.sizeDelta.y) / barSpeed;
		isActive = true;
	}


	// Create the bars of the cinematic cam
	private void CreateBars()
	{
		GameObject barsObject = new GameObject("topBar", typeof(Image));
		barsObject.transform.SetParent(transform, false); // Scales the parent size in order to maintain this objects size.
		barsObject.GetComponent<Image>().color = Color.black;
		topBar = barsObject.GetComponent<RectTransform>();
		topBar.anchorMin = new Vector2(ANCHOR_MIN, ANCHOR_MAX);
		topBar.anchorMax = new Vector2(ANCHOR_MAX, ANCHOR_MAX);
		topBar.sizeDelta = new Vector2(ANCHOR_MIN, ANCHOR_MIN);

		barsObject = new GameObject("bottomBar", typeof(Image));
		barsObject.transform.SetParent(transform, false);
		barsObject.GetComponent<Image>().color = Color.black;
		bottomBar = barsObject.GetComponent<RectTransform>();
		bottomBar.anchorMin = new Vector2(ANCHOR_MIN, ANCHOR_MIN);
		bottomBar.anchorMax = new Vector2(ANCHOR_MAX, ANCHOR_MIN);
		bottomBar.sizeDelta = new Vector2(ANCHOR_MIN , ANCHOR_MIN);
	}
}