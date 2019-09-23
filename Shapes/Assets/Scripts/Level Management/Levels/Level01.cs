/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is the mission controller for Level01.
* Attach this to an empty object in Level01 scene.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Level01 : MonoBehaviour
{
	// Components
	private AudioSource audioSource;

	// GameObjects
	public AudioClip intro;
	public AudioClip breakingFree;

	// Global Variables
	[SerializeField]
	private float cameraIntroXAxis = 90f;
	[SerializeField]
	private float cameraIntroYAxis = 12f;

	// ============================================================
	// MonoBehaviour Methods (In order of execution)
	// ============================================================

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
	}

	private void OnEnable()
	{
		BreakableGround.BrokeGround += PlayBreakingFreeMusic;
	}

	private void Start()
	{
		CameraController.Instance.SetCameraPosition(cameraIntroXAxis, cameraIntroYAxis);
		CameraController.Instance.PlayCutscene(CameraController.Cutscenes.Level01Intro);
		audioSource.clip = intro;
		audioSource.Play();
	}

	private void OnDisable()
	{
		BreakableGround.BrokeGround -= PlayBreakingFreeMusic;
	}

	// ============================================================
	// Level01 Methods
	// ============================================================

	private void PlayBreakingFreeMusic()
	{
		if(audioSource != null)
		{
			audioSource.clip = breakingFree;
			audioSource.volume = 0.6f;
			audioSource.Play();
		}
	}
}
