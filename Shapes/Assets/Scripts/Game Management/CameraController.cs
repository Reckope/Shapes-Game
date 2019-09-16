using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class CameraController : MonoBehaviour
{

	// There can only ever be one instance of the camera..
	public static CameraController Instance { get { return _instance; } }
	private static CameraController _instance;

	private CinematicBars CinematicBars;

	private Animator Animator;

	private Transform player;
	private Camera Camera;

	// Global Variables
	public bool followPlayer;
	private const float FOLLOW_PLAYER_DAMP_TIME = 0.25f;
	private const float CAMERA_DELTA_X_POSITION = 0.5f;
	private const float CAMERA_DELTA_Y_POSITION = 0.5f;
	[SerializeField][Range(-1f, 1f)]
	private float distanceAheadOfPlayer = 1f;
	[SerializeField][Range(-1f, 3f)]
	private float distanceAbovePlayer = 1f;
	private float cameraMaxXBounds;
	private float cameraMinXBounds;
	private float cameraMinYBounds;
	private float cameraMaxYBounds;
	private Vector3 velocity = Vector3.zero;

	private void Awake()
	{
		CinematicBars = GameObject.FindObjectOfType(typeof(CinematicBars)) as CinematicBars;
		player = GameObject.FindGameObjectWithTag("Player").transform;
		Camera = GetComponent<Camera>();
		Animator = GetComponent<Animator>();
		Assert.IsNotNull(player);
		Assert.IsNotNull(CinematicBars);
		Assert.IsNotNull(Camera);
		Assert.IsNotNull(Animator);
	}

	void OnEnable()
	{
		followPlayer = true;
		GameManager.Instance.GamePaused += PauseMusic;
		LevelCompleteTrigger.CompletedLevel += OnFollowPlayer;
		Level04.PlayLevel04Intro += LevelFourIntro;
		Level01.PlayLevel01Intro += LevelOneIntro;
	}

	void OnDisable()
	{
		GameManager.Instance.GamePaused -= PauseMusic;
		LevelCompleteTrigger.CompletedLevel -= OnFollowPlayer;
		Level04.PlayLevel04Intro -= LevelFourIntro;
		Level01.PlayLevel01Intro -= LevelOneIntro;
	}

	// Start is called before the first frame update
	void Start()
	{
		cameraMaxYBounds = 9999f;
		cameraMinYBounds = -9999f;
		cameraMinXBounds = -9999f;
		cameraMaxXBounds = 9999f;
		Instanced();
	}

	private void Instanced()
	{
		if(_instance != null && _instance != this)
		{
			Debug.Log("Error: Another instance of Camera has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}
	}

	// Update is called once per frame
	private void Update()
	{
		if (player != null && !Player.Instance.IsDead && followPlayer){
			FollowPlayer();
		}
	}

	// Constantly follow the player throughout the game, then stop when reaching
	// the edge of the game world. 
	private void FollowPlayer()
	{
		followPlayer = true;
		Animator.enabled = false;
		// Set position
		Vector3 point = Camera.WorldToViewportPoint(player.position);
		Vector3 delta = player.position - Camera.ViewportToWorldPoint(new Vector3(CAMERA_DELTA_X_POSITION, CAMERA_DELTA_Y_POSITION, point.z));
		Vector3 destination = transform.position + delta;
		// Set Bounds
		destination.x = Mathf.Clamp (destination.x + distanceAheadOfPlayer, cameraMinXBounds, cameraMaxXBounds);
		destination.y = Mathf.Clamp (destination.y + distanceAbovePlayer, cameraMinYBounds, cameraMaxYBounds);
		// Follow
		transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, FOLLOW_PLAYER_DAMP_TIME);
	}

	private void OnFollowPlayer(int level, bool completed)
	{
		followPlayer = false;
	}

	private void LevelFourIntro()
	{
		followPlayer = false;
		Animator.enabled = true;
		transform.position = new Vector3(90f, 28f, transform.position.z);
		Animator.SetBool("playLevelFourIntro", true);
		Invoke("FollowPlayer", 35.3f);
	}

	private void LevelOneIntro()
	{
		followPlayer = false;
		Animator.enabled = true;
		transform.position = new Vector3(48f, 3.7f, transform.position.z);
		Animator.SetBool("playLevelOneIntro", true);
		Invoke("FollowPlayer", 15.0f);
	}

	private void PauseMusic()
	{
		//AudioListener.pause = true;
	}
}
