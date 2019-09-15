using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
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
	private float cameraDistanceAheadOfPlayer;
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
		Animator.enabled = false;
		followPlayer = true;
	}

	void OnEnable()
	{
		LevelCompleteTrigger.CompletedLevel += OnFollowPlayer;
		Level04.PlayLevel04Intro += LevelFourIntro;
	}

	void OnDisable()
	{
		LevelCompleteTrigger.CompletedLevel -= OnFollowPlayer;
		Level04.PlayLevel04Intro -= LevelFourIntro;
	}

	// Start is called before the first frame update
	void Start()
	{
		cameraMaxYBounds = 9999f;
		cameraMinYBounds = -9999f;
		cameraMinXBounds = -9999f;
		cameraMaxXBounds = 9999f;
		cameraDistanceAheadOfPlayer = 1f;
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
		destination.x = Mathf.Clamp (destination.x + cameraDistanceAheadOfPlayer, cameraMinXBounds, cameraMaxXBounds);
		destination.y = Mathf.Clamp (destination.y + distanceAheadOfPlayer, cameraMinYBounds, cameraMaxYBounds);
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
}
