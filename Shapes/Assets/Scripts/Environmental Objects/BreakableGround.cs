/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to let the game know when ground has been broken.
* Attach this to Breakable Ground grid.
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class BreakableGround : MonoBehaviour
{
	// Components
	private AudioSource audioSource;

	// Events
	public static event Action BrokeGround;

	// Global Variables
	[Header("Breakable Ground Settings")]
	[SerializeField][Range(0.1f, 1f)]
	private float breakAudioVolume = 0.5f;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Awake()
	{
		audioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(audioSource);
	}

	private void Start()
	{
		this.gameObject.SetActive(true);
	}

	// When ground gets hit by the block.
	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == Ped.States.Block.ToString())
		{
			StartCoroutine(BreakGround());
			if(BrokeGround != null)
			{
				BrokeGround();
			}
		}
	}

	// =========================================================
	// Breakable Ground Methods
	// =========================================================

	// Move the ground off screen before destroying, so the 
	// audio plays correctly.
	private IEnumerator BreakGround()
	{
		int offScreenPosition = 999;
		float secondsBeforeBeingDestroyed = 5f;

		audioSource.volume = breakAudioVolume;
		audioSource.Play();
		transform.position = new Vector2(offScreenPosition, offScreenPosition);
		yield return new WaitForSeconds(secondsBeforeBeingDestroyed);
		this.gameObject.SetActive(false);
	}
}
