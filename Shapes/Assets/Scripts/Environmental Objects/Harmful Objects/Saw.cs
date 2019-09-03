/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the saw.
* Attach this to the saw object.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
	// Global Variables
	[Header("Saw Settings")]
	[SerializeField] [Range(1, 10)]
	private float rotationSpeed = 1;
	private float stationary = 0;

	void Start()
	{
		if (rotationSpeed == stationary){
			rotationSpeed = 1f;
		}
	}

	void FixedUpdate()
	{
		transform.Rotate(stationary, stationary, -rotationSpeed);
	}
}
