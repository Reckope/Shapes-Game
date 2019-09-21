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
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Saw : MonoBehaviour
{
	// Components
	private AudioSource audioSource;

	// Global Variables
	[Header("Saw Settings")]
	[SerializeField][Range(1f, 5f)]
	private float rotationSpeed = 1;
	private const float STATIONARY = 0;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
	}

	private void Start()
	{
		Assert.AreNotEqual(rotationSpeed, 0);
	}

	private void FixedUpdate()
	{
		transform.Rotate(STATIONARY, STATIONARY, -rotationSpeed);
	}

	// When a ped collides with the saw..
	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.GetComponent<Ped>())
		{
			audioSource.Play();
		}
	}
}
