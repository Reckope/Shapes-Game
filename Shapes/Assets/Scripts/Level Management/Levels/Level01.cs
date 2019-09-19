using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class Level01 : Level
{
	private AudioSource audioSource;

	public AudioClip intro;
	public AudioClip breakingFree;

	public static event Action PlayLevel01Intro;

	private void OnEnable()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
		BreakableGround.BrokeGround += PlayBreakingFreeMusic;
	}

	// Start is called before the first frame update
	void Start()
	{
		if(PlayLevel01Intro != null)
		{
			PlayLevel01Intro();
		}
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
