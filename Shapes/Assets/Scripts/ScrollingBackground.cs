﻿/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to instantiate, move and reposition a menu background image.
* If you want a moving background, attach this to an empty MainMenuController GameObject, and 
* all it requires is for a designer to add a background sprite via the unity inspector. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrollingBackground : MonoBehaviour
{
	// GameObjects
	public Transform backgroundImage;
	private Transform cloneBackgroundImage;

	// Components
	private SpriteRenderer backgroundSprite;
	private Rigidbody2D rb2d;
	private Rigidbody2D cloneRb2d;

	// Global Variables
	[SerializeField][Range(0.1f, 10.0f)]
	private float movementSpeed = 0.1f;
	private const int HORIZONTAL_MOVEMENT = 0;

	// ------------------------------------------------------------------------------
	private void Awake()
	{
		if(backgroundImage != null)
		{
			if(backgroundImage.GetComponent<Rigidbody2D>() == null)
			{
				rb2d = backgroundImage.gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
			}
			rb2d = backgroundImage.GetComponent<Rigidbody2D>();
			rb2d.bodyType = RigidbodyType2D.Kinematic;
			rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
			backgroundSprite = backgroundImage.GetComponent<SpriteRenderer>();
			DuplicateBackgroundImage();
		}
		else
		{
			Debug.LogError("Error: Missing Background Image. Please add a Background Image via the Inspector.");
		}
	}

	private void FixedUpdate()
	{
		MoveBackground();
	}

	private void Update()
	{
		if(backgroundImage.transform.position.x <= RepositionPoint())
		{
			RepositionBackground(backgroundImage);
		}
		else if(cloneBackgroundImage.transform.position.x <= RepositionPoint())
		{
			RepositionBackground(cloneBackgroundImage);
		}
	}

	// Use Mathf.Floor to round down to the largest integer (20.7f -> 20).
	// This is to prevent gaps appearing between background images.
	private float LengthOfSprite()
	{
		float length;

		length = backgroundSprite.bounds.size.x;
		return Mathf.Floor(length);
	}

	private float RepositionPoint()
	{
		float point;

		point = -LengthOfSprite();
		return point;
	}

	// Clone backgroundImage and position it to edge of it's original.
	private void DuplicateBackgroundImage()
	{
		cloneBackgroundImage = Instantiate(backgroundImage);
		cloneBackgroundImage.transform.position = new Vector2(backgroundImage.transform.position.x + LengthOfSprite(), backgroundImage.transform.position.y);
		cloneRb2d = cloneBackgroundImage.GetComponent<Rigidbody2D>();
	}

	private void MoveBackground()
	{
		rb2d.velocity = new Vector2 (-movementSpeed, HORIZONTAL_MOVEMENT);
		cloneRb2d.velocity = new Vector2(-movementSpeed, HORIZONTAL_MOVEMENT);
	}

	private void RepositionBackground(Transform background)
	{
		background.transform.position = new Vector2(LengthOfSprite(), HORIZONTAL_MOVEMENT);
	}
}