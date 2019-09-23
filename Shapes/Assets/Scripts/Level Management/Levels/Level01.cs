using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Level01 : MonoBehaviour
{
	private AudioSource audioSource;

	public AudioClip intro;
	public AudioClip breakingFree;

	private void OnEnable()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
		BreakableGround.BrokeGround += PlayBreakingFreeMusic;
	}

	void Start()
	{
		CameraController.Instance.SetCameraPosition(90f, 28f);
		CameraController.Instance.PlayCutscene("Level01Intro");
		audioSource.clip = intro;
		audioSource.Play();
	}

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
