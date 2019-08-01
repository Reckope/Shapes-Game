using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
[RequireComponent(typeof(Animator))]
public class Ped : MonoBehaviour
{
	// Classes
	protected StateMachine stateMachine;

	// Components
	private Rigidbody2D rb2d;
	private Collider2D Collider2D;
	private Animator anim;

	// Global Variables
	private string pedName;
	private string sound;
	private float speed;

	// ------------------------------------------------------------------------------
	// Ped Name
	public void setName(string newName)
	{
		pedName = newName;
	}

	public string getName()
	{
		return pedName; 
	}

	// Ped Sound
	public void setSound(string newSound)
	{
		sound = newSound;
	}

	public string getSound()
	{
		return sound; 
	}

	// Ped Movement Speed
	public void setSpeed(float newSpeed)
	{
		speed = newSpeed;
	}

	public float getSpeed()
	{
		return speed; 
	}

	// ---------- Components ----------
	// Any ped that inherits this class, can simply set and get the components they need
	// from here instead of initializing them everytime. 
	public void SetRigidBody2D()
	{
		rb2d = GetComponent<Rigidbody2D>();
	}

	public Rigidbody2D GetRigidBody2D()
	{
		return rb2d;
	}

	public void SetCollider2D()
	{
		Collider2D = GetComponent<Collider2D>();
	}

	public Collider2D GetCollider2D()
	{
		return Collider2D;
	}

	public void SetAnimator()
	{
		anim = GetComponent<Animator>();
	}

	public Animator GetAnimator()
	{
		return anim;
	}
}
