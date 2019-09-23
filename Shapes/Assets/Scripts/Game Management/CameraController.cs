/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the main Camera.
* Attach this directly to the Camera GameObject.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{
	// There can only ever be one instance of the camera.
	public static CameraController Instance { get { return _instance; } }
	private static CameraController _instance;

	public enum Cutscenes
	{
		Level01Intro,
		Level04Intro
	}

	// Classes
	private CinematicBars CinematicBars;

	// Components
	private Animator Animator;
	private AudioLowPassFilter AudioLowPassFilter;

	// GameObjects
	private Transform player;
	private Camera Camera;

	// Events
	public static event Action CutsceneIsStarting;
	public static event Action<string> CutsceneIsActive;
	public static event Action CutsceneIsFinished;

	// Global Variables
	private bool followPlayer;

	[SerializeField]
	private float followPlayerDampTime = 0.25f;
	private float cameraDeltaXPosition = 0.5f;
	private float cameraDeltaYPosition = 0.5f;

	[SerializeField][Range(-1f, 1f)]
	private float distanceAheadOfPlayer = 1f;
	private float distanceAhead;
	[SerializeField][Range(-1f, 3f)]
	private float distanceAbovePlayer = 1f;

	private Vector3 velocity = Vector3.zero;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Awake()
	{
		Instanced();
		CinematicBars = GameObject.FindObjectOfType(typeof(CinematicBars)) as CinematicBars;
		Assert.IsNotNull(CinematicBars);
		player = GameObject.FindGameObjectWithTag(Ped.PedType.Player.ToString()).transform;
		Camera = GetComponent<Camera>();
		Assert.IsNotNull(Camera);
		Animator = GetComponent<Animator>();
		Assert.IsNotNull(Animator);
		AudioLowPassFilter = GetComponent<AudioLowPassFilter>();
		Assert.IsNotNull(AudioLowPassFilter);
	}

	private void OnEnable()
	{
		followPlayer = true;
		GameManager.Instance.ActionShotIsActive += Action;
		LevelCompleteTrigger.LevelIsComplete += StopFollowingPlayer;
		Player.HasDied += MuffleMusic;
	}

	private void Start()
	{
		AudioLowPassFilter.enabled = false;
		distanceAhead = distanceAheadOfPlayer;
	}

	private void Update()
	{
		if (player != null && !Player.Instance.IsDead && followPlayer)
		{
			SeeAheadOfPlayer();
			FollowPlayer();
		}
	}

	private void OnDisable()
	{
		GameManager.Instance.ActionShotIsActive -= Action;
		LevelCompleteTrigger.LevelIsComplete -= StopFollowingPlayer;
		Player.HasDied -= MuffleMusic;
	}

	// =========================================================
	// Camera Methods
	// =========================================================

	private void Instanced()
	{
		if(_instance != null && _instance != this)
		{
			Debug.Log("Eror: Another instance of Camera has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}
	}

	// Used in Update()
	private void SeeAheadOfPlayer()
	{
		if(Player.Instance.FaceDirection >= 0)
		{
			distanceAheadOfPlayer = distanceAhead;
		}
		else
		{
			distanceAheadOfPlayer = -distanceAhead;
		}
	}

	// Used in Update()
	// Also used by Animation events (for cutscenes) :)
	private void FollowPlayer()
	{
		// Set position
		Vector3 point = Camera.WorldToViewportPoint(player.position);
		Vector3 delta = player.position - Camera.ViewportToWorldPoint(new Vector3(cameraDeltaXPosition, cameraDeltaYPosition, point.z));
		Vector3 destination = transform.position + delta;
		// Change offset
		destination.x += distanceAheadOfPlayer;
		destination.y += distanceAbovePlayer;
		// Follow
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, followPlayerDampTime);
	}

	private void StopFollowingPlayer()
	{
		followPlayer = false;
	}

	// These methods are used by mission controllers for cutscenes.

	public void SetCameraPosition(float xAxis, float yAxis)
	{
		transform.position = new Vector3(xAxis, yAxis, transform.position.z);
	}

	public void PlayCutscene(Cutscenes scene)
	{
		string _scene = scene.ToString();

		CinematicBars.ShowCinematicBars(1.3f);
		if(CutsceneIsStarting != null)
		{
			CutsceneIsStarting();
		}
		if(CutsceneIsActive != null)
		{
			CutsceneIsActive(_scene);
		}
		followPlayer = false;
		Animator.enabled = true;
		Animator.Play(_scene, 0);
	}

	// Used by Animation event
	private void FinishCutscene()
	{
		CinematicBars.HideCinematicBars(0.8f);
		if(CutsceneIsFinished != null)
		{
			CutsceneIsFinished();
		}
		Animator.enabled = false;
		followPlayer = true;
	}

	// End of Mission Controller methods.

	// These methods are used by events.

	private void Action(bool actionIsEnabled)
	{
		if(actionIsEnabled)
		{
			Camera.orthographicSize = 4.8f;
		}
		else
		{
			Camera.orthographicSize = 5.5f;
		}
	}

	public void MuffleMusic()
	{
		AudioLowPassFilter.enabled = true;
	}
}