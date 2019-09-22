/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control a variety of music throughout the game.
* Attach this directly to the GameManager GameObject, so it's not destroyed.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class AudioManager : MonoBehaviour
{
	// Components
	private AudioSource audioSource;

	// Audio Clips
	public AudioClip menuMusic;
	public AudioClip gameOverMusic;

	// Global Variables
	[SerializeField]
	private float fadeOutMusicTime = 1f;
	private const float MUTE = 0;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
	}

	private void OnEnable()
	{
		SceneController.LoadedScene += EnableAllAudio;
		Player.HasDied += PlayGameOver;
		LevelCompleteTrigger.LevelIsComplete += FadeMusic;
		LevelManager.LevelHasActivated += HandleMenuMusic;
	}

	private void Start()
	{
		StartMenuMusic(true);
		StopCoroutine(FadeOutMusic());
	}

	private void OnDisable()
	{
		SceneController.LoadedScene -= EnableAllAudio;
		Player.HasDied -= PlayGameOver;
		LevelCompleteTrigger.LevelIsComplete -= FadeMusic;
		LevelManager.LevelHasActivated -= HandleMenuMusic;
	}

	// =========================================================
	// Audio Manager Methods (Usually called by the events)
	// =========================================================

	private void HandleMenuMusic(bool levelIsActive)
	{
		if(levelIsActive)
		{
			StartMenuMusic(false);
		}
		else
		{
			StartMenuMusic(true);
		}
	}

	private void StartMenuMusic(bool start)
	{
		audioSource.clip = menuMusic;
		Assert.IsNotNull(menuMusic);
		audioSource.loop = true;

		if(start)
		{
			audioSource.Play();
		}
		else
		{
			audioSource.Stop();
		}
	}

	private void PlayGameOver()
	{
		audioSource.clip = gameOverMusic;
		Assert.IsNotNull(gameOverMusic);
		audioSource.loop = false;
		audioSource.Play();
	}

	private void EnableAllAudio()
	{
		AudioListener.pause = false;
		AudioListener.volume = 1f;
	}

	// Wrapper for event.
	// Otherwise, would have to make the event type IEnumerator, which
	// can mess up other non-IEnumerator classes that subscribed to the event.
	private void FadeMusic()
	{
		StartCoroutine(FadeOutMusic());
	}

	private IEnumerator FadeOutMusic () 
	{
		float startVolume = AudioListener.volume;
 
		while (AudioListener.volume > MUTE) 
		{
			AudioListener.volume -= startVolume * Time.deltaTime / fadeOutMusicTime;
			yield return null;
		}
 
		AudioListener.pause = true;
	}
}