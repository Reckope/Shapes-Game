/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the player.
* Attach this to the player GameObject,
* Derived Ped (here) > Ped > Statemachine > State > SomeState
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Ped
{
	// There can only ever be one instance of a player.
	public static Player Instance { get { return _instance; } }
	private static Player _instance;

	private CinematicBars CinematicBars;

	public static event Action HasDied;

	// ============================================================
	// Player Variables
	// ============================================================

	[Header("Player Settings")]
	[SerializeField]
	private PedNames _name = PedNames.Morphy;
	[SerializeField][Range(0.1f, 15.0f)]
	private float _speed = 0.1f, _jumpForce = 0.1f, blockCheckRadius = 1.4f;
	public bool isDead, isInvulnerable, inputIsEnabled = true;
	public int Lives { get; set; }
	public int MaxLives { get; set; }

	private bool levelFourIsActive;

	[SerializeField][Range(0.1f, 0.5f)]
	private float _sideCheckRadius = 0.1f, _groundCheckRadius = 0.2f;

	[Header("Player Components & GameObjects")]
	public Transform morphIntoBlockCheck;
	public GameObject blockFeedback;
	public bool CanMorphIntoBlock 
	{ 
		get { return !Physics2D.OverlapCircle (morphIntoBlockCheck.position, blockCheckRadius, whatIsGround); } 
	}
	public GameObject[] hearts;
	
	public AudioClip footStepsAudio;
	private AudioSource audioSource;

	public event Action<int> OnLivesChanged;

	// ============================================================
	// MonoBehaviour methods
	// ============================================================

	// We override all of the peds' MonoBehaviour methods to 
	// inherit any components, methods etc, whilst adding extra code here. 
	protected override void Awake()
	{
		base.Awake();

		CinematicBars = GameObject.FindObjectOfType(typeof(CinematicBars)) as CinematicBars;
		audioSource = GetComponent<AudioSource>();
		if(_instance != null && _instance != this)
		{
			Debug.Log("Error: Another instance of Player has been found in scene " + " '" + SceneController.GetActiveScene() + "'.");
			Destroy(this.gameObject);
		} 
		else
		{
			_instance = this;
		}

		pedType = PedType.Player;
		Name = _name.ToString();
		GroundCheckRadius = _groundCheckRadius;
		SideCheckRadius = _sideCheckRadius;
		blockFeedback.SetActive(false);
	}

	private void OnEnable()
	{
		LevelCompleteTrigger.LevelIsComplete += CompletedLevel;
		Level04.PlayLevel04Intro += RollIntoLevel;
		Level01.PlayLevel01Intro += LevelOneIntro;
	}

	private void OnDisable()
	{
		LevelCompleteTrigger.LevelIsComplete -= CompletedLevel;
		Level04.PlayLevel04Intro -= RollIntoLevel;
		Level01.PlayLevel01Intro -= LevelOneIntro;
	}

	protected override void Start()
	{
		base.Start();

		stateMachine.SetState(new IdleState(stateMachine, this));
		Lives = 3;
		MaxLives = 3;
		isInvulnerable = false;
		OnLivesChanged += DisplayLives;
		DisplayLives(Lives);
	}

	protected override void Update()
	{
		base.Update();

		isDead = IsDead;
		Speed = _speed;
		JumpForce = _jumpForce;
		if(!isDead && inputIsEnabled)
		{
			HandlePlayerInput();
		}

		if(HasMorphed)
		{
			DisplayLives(0);
		}
		else
		{
			DisplayLives(Lives);
		}

		if(levelFourIsActive)
		{
			AudioSource.volume = 0;
			MorphToBallInput = true;
			SetPedState(States.Ball);
			inputIsEnabled = false;
			MovementDirection = (int)Direction.Right;
		}

	}

	// ============================================================
	// Player Input
	// ============================================================

	private void HandlePlayerInput()
	{
		bool MorphToBlockFeedbackInput = Input.GetKey("up");
		bool jump = Input.GetKeyDown("w");

		MovementDirection = Input.GetAxisRaw("Horizontal");

		// Returns true every frame. This is used to detect when the 
		// player has released the key associated with the state.
		MorphToBallInput = Input.GetKey("down");
		MorphToBlockInput = Input.GetKey("up");
		MorphToHorizontalShieldInput = Input.GetKey("right");
		MorphToVerticalShieldInput = Input.GetKey("left");

		// The player can enter the following morph states.
		if(IsGrounded)
		{
			if(MorphToHorizontalShieldInput)
			{
				SetPedState(States.HorizontalShield);
			}
			if(MorphToVerticalShieldInput)
			{
				SetPedState(States.VerticalShield);
			}
			if(jump)
			{
				// Putting the sound here so it doesn't get repeatedly called
				// for a few frames in ped.cs FixedUpdate.
				PlaySound(PedSounds.Jump, true, false, 1);
				Jumped = true;
			}
		}
		else
		{
			if(MorphToBlockInput && DistanceFromGround() > 3f && CanMorphIntoBlock)
			{
				blockFeedback.SetActive(false);
				SetPedState(States.Block);
			}
		}

		if(MorphToBallInput)
		{
			SetPedState(States.Ball);
		}

		if(MorphToBlockFeedbackInput && !CanMorphIntoBlock && !HasMorphed)
		{
			blockFeedback.SetActive(true);
		}

		if(!MorphToBlockFeedbackInput)
		{
			blockFeedback.SetActive(false);
		}
	}

	// ============================================================
	// Player Related tasks.
	// ============================================================

	private void CompletedLevel()
	{
		isInvulnerable = true;
		inputIsEnabled = false;
		IsAbleToJump = false;
	}

	public void LoseLives(int life)
	{
		if(Lives - life > 0) 
		{
			Lives -= life;
			StartCoroutine(BecomeTemporarilyInvulnerable(1));
		}
		else
		{
			if(HasDied != null)
			{
				HasDied();
			}
			Lives = 0;
			StartCoroutine(PlayerDied());
			Die();
		}

		if (OnLivesChanged != null)
		{
			OnLivesChanged(Lives);
		}
	}

	public void IncreaseLives(int life)
	{
		if(Lives + life <= MaxLives) 
		{
			Lives += life;
			if (OnLivesChanged != null)
			{
				OnLivesChanged(Lives);
			}
		}
	}

	private void DisplayLives(int lives)
	{
		for(int i = 0; i < hearts.Length; i++)
		{
			if(i < lives && lives != 0)
			{
				hearts[i].SetActive(true);
			}
			else
			{
				hearts[i].SetActive(false);
			}
		}
	}

	private IEnumerator BecomeTemporarilyInvulnerable(int seconds)
	{
		isInvulnerable = true;
		yield return new WaitForSeconds(seconds);
		isInvulnerable = false;
	}

	public IEnumerator PlayerDied()
	{
		GameManager.Instance.EnableSlowMotion(true);
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.PlayerDiedFilter, true);
		yield return new WaitForSeconds(1.1f);
		GameManager.Instance.EnableSlowMotion(false);
		UIManager.Instance.DisplayUI(UIManager.CanvasNames.PlayerDiedButtons, true);
	}

	private void RollIntoLevel()
	{
		if(CinematicBars != null)
		{
			CinematicBars.ShowCinematicBars();
		}
		// In the release build, the player would not roll at the start of the level.
		// I had to add a bool here and add it to Update() to ensure it gets called.
		// No idea why it worked in the editor, but not in release. 
		if(this != null)
		{
			transform.position = new Vector2(-580.5f, -2.44f);
			levelFourIsActive = true;
			Invoke("ReturnToNormal", 52.6f);
		}
	}

	private void LevelOneIntro()
	{
		if(CinematicBars != null)
		{
			CinematicBars.ShowCinematicBars();
		}
		inputIsEnabled = false;
		MovementDirection = (int)Direction.Idle;
		Invoke("ReturnToNormal", 15f);
	}

	private void ReturnToNormalLevelOne()
	{
		inputIsEnabled = true;
	}

	private void ReturnToNormal()
	{
		AudioSource.volume = 1f;
		levelFourIsActive = false;
		CinematicBars.HideCinematicBars();
		inputIsEnabled = true;
		MorphToBallInput = false;
		MovementDirection = (int)Direction.Idle;
	}

	public void PlayFootStepsAudio()
	{
		audioSource.clip = footStepsAudio;
		audioSource.Play();
	}
}
