using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(StateMachine))]
public class Ped : MonoBehaviour
{
	// Classes
	protected StateMachine stateMachine;

	// Components
	public Rigidbody2D Rigidbody2D;
	public Collider2D Collider2D;
	public Animator Animator;

	// GameObjects / Transform
	public LayerMask whatIsGround;
	public Transform groundCheck;

	// Global Variables
	public bool grounded, jumped, moving;
	private string pedName;
	private string sound;
	private float speed;
	private float jumpForce;

	// *** JOE. If multiple peds are acting the same, create a constructor and use:
	// Ped player = new ped(name, speed etc) ***
	// ============================================================
	// MonoBehaviour methods.
	// ============================================================

	protected virtual void Awake()
	{
		Rigidbody2D = GetComponent<Rigidbody2D>();
		Collider2D = GetComponent<Collider2D>();
		Animator = GetComponent<Animator>();
		stateMachine = GetComponent<StateMachine>();
	}

	protected virtual void Start()
	{
		Rigidbody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
		Rigidbody2D.bodyType = RigidbodyType2D.Dynamic;
		stateMachine.SetState(new IdleState(stateMachine, this));
	}

	protected virtual void Update()
	{
		Debug.Log(stateMachine.GetCurrentState());
		stateMachine.UpdateState();
		FlipSprite();
		float groundCheckRadius = 0.3f;
		Grounded = Physics2D.OverlapCircle (groundCheck.position, groundCheckRadius, whatIsGround);
	}

	protected virtual void FixedUpdate()
	{
		stateMachine.FixedUpdateState();
	}

	// ============================================================
	// Get Set methods / properties.
	// ============================================================

	// Is the ped grounded or not?
	public bool Grounded
	{
		get
		{
			return grounded;
		}
		set
		{
			if(value == grounded)
			{
				return;
			}
			stateMachine.SetState(new AirbornState(stateMachine, this));
			grounded = value;
			if(grounded)
			{
				stateMachine.SetState(new GroundedState(stateMachine, this));
			}
		}
	}

	// Ped Name
	public string GetName(){ return pedName; }
	public void SetName(string newName){ pedName = newName; }

	// Ped Sound
	public string GetSound(){ return sound; }
	public void SetSound(string newSound){ sound = newSound; }

	// Ped Movement Speed
	public float GetSpeed(){ return speed; }
	public void SetSpeed(float newSpeed){ speed = newSpeed; }

	// Jump Force
	public float GetJumpForce(){ return jumpForce; }
	public void SetJumpForce(float newJumpForce){ jumpForce = newJumpForce; }

	// ============================================================
	// Ped Tasks
	// ============================================================

	private void FlipSprite()
	{
		if(Rigidbody2D.velocity.x > 0)
		{
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		else if(Rigidbody2D.velocity.x < 0)
		{
			transform.eulerAngles = new Vector3(0, 180, 0);
		}
	}

	public void Jump()
	{
		Animator.SetTrigger("takeOff");
		Rigidbody2D.velocity = Vector2.up * jumpForce;
		jumped = false;
	}

	public void Walk()
	{
		Rigidbody2D.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * GetSpeed(), Rigidbody2D.velocity.y);
	}
}