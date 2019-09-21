// ATTACH THIS TO THE GAMEMANAGER OBJECT

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource audioSource;
	public AudioClip menuMusic;
	public AudioClip gameOverMusic;

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void Start()
	{
		StartMenuMusic(true);
		StopCoroutine(FadeOutMusic(1));
	}

	private void OnEnable()
	{
		SceneController.LoadedScene += ListenToAllAudio;
		Player.HasDied += PlayGameOver;
		LevelCompleteTrigger.LevelIsComplete += FadeMusic;
		LevelManager.LevelHasActivated += HandleMenuMusic;
	}

	private void OnDisable()
	{
		SceneController.LoadedScene -= ListenToAllAudio;
		Player.HasDied -= PlayGameOver;
		LevelCompleteTrigger.LevelIsComplete -= FadeMusic;
		LevelManager.LevelHasActivated -= HandleMenuMusic;
	}

	private void HandleMenuMusic(bool levelIsActive)
	{
		if(!levelIsActive)
		{
			StartMenuMusic(true);
		}
		else
		{
			StartMenuMusic(false);
		}
	}

	private void StartMenuMusic(bool start)
	{
		audioSource.clip = menuMusic;
		audioSource.loop = start;
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
		audioSource.loop = false;
		audioSource.Play();
	}

	private void ListenToAllAudio()
	{
		AudioListener.pause = false;
		AudioListener.volume = 1f;
	}

	// Wrapper
	private void FadeMusic()
	{
		StartCoroutine(FadeOutMusic(1));
	}

	private IEnumerator FadeOutMusic (float FadeTime) {
		float startVolume = AudioListener.volume;
 
		while (AudioListener.volume > 0) {
			AudioListener.volume -= startVolume * Time.deltaTime / FadeTime;
			yield return null;
		}
 
		AudioListener.pause = true;
	}
}
