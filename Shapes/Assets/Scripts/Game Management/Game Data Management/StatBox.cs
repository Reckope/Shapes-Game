/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to display the values in the stats box's.
* Attach this to the Statbox prefab.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBox : MonoBehaviour
{
	// ============================================================
	// Global Variables & Objects
	// ============================================================

	// Text objects
	public Text nameText;
	public Text valueText;

	// Variables
	public string ID { get; set; }
	public string Name { get; set; }
	public float Value { get; set; }

	private const float MINUTES_IN_ONE_HOUR = 60f;
	private const float SECONDS_IN_ONE_HOUR = 3600f;

	// ============================================================
	// MonoBehaviour Methods
	// ============================================================

	void Start()
	{
		nameText.text = Name;
		if(ID == GameData.PlayerStatIDs.TimePlayed.ToString())
		{
			DisplayTimeFormat();
		}
		else
		{
			valueText.text = Value.ToString();
		}
	}

	// ============================================================
	// StatBox Methods
	// ============================================================

	private void DisplayTimeFormat()
	{
		int hours = Mathf.FloorToInt(Value / SECONDS_IN_ONE_HOUR);
		int minutes = Mathf.FloorToInt(Value / MINUTES_IN_ONE_HOUR);
		int seconds = Mathf.FloorToInt(Value - minutes * MINUTES_IN_ONE_HOUR);

		valueText.text = string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds);
	}
}