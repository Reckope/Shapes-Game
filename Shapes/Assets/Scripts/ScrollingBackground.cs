/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to instantiate, move and reposition a menu background image.
* If you want a moving background, attach this to an empty MainMenuController GameObject, and 
* all it requires is for a designer to add a background sprite via the inspector. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ScrollingBackground : MonoBehaviour
{
	// Components and GameObjects
	public Transform backgroundImage;
	private Transform dupeBackgroundImage;
	private SpriteRenderer backgroundSprite;
	private Rigidbody2D rb2d;
	private Rigidbody2D dupeRb2d;

	// Global Variables
	private const int HORIZONTAL_MOVEMENT = 0;
	[SerializeField][Range(0.1f, 10.0f)]
	private float movementSpeed;

	// ------------------------------------------------------------------------------
	void Start()
	{
		if(backgroundImage != null)
		{
			if(backgroundImage.GetComponent<Rigidbody2D>() == null)
			{
				rb2d = gameObject.AddComponent(typeof(Rigidbody2D)) as Rigidbody2D;
			}
			rb2d = backgroundImage.GetComponent<Rigidbody2D>();
			rb2d.bodyType = RigidbodyType2D.Kinematic;
			rb2d.interpolation = RigidbodyInterpolation2D.Interpolate;
			backgroundSprite = backgroundImage.GetComponent<SpriteRenderer>();
			DuplicateBackgroundImage();
		}
		else
		{
			throw new Exception("Error: Missing Background Image. Please add a Background Image via the Inspector.");
		}
	}

	void FixedUpdate()
	{
		MoveBackground();
		if(backgroundImage.transform.position.x <= RepositionPoint())
		{
			RepositionBackground(backgroundImage);
		}
		else if(dupeBackgroundImage.transform.position.x <= RepositionPoint())
		{
			RepositionBackground(dupeBackgroundImage);
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
		dupeBackgroundImage = Instantiate(backgroundImage);
		dupeBackgroundImage.transform.position = new Vector2(backgroundImage.transform.position.x + LengthOfSprite(), backgroundImage.transform.position.y);
		dupeRb2d = dupeBackgroundImage.GetComponent<Rigidbody2D>();
	}

	private void MoveBackground()
	{
		rb2d.velocity = new Vector2 (-movementSpeed, HORIZONTAL_MOVEMENT);
		dupeRb2d.velocity = new Vector2(-movementSpeed, HORIZONTAL_MOVEMENT);
	}

	private void RepositionBackground(Transform background)
	{
		background.transform.position = new Vector2(LengthOfSprite(), HORIZONTAL_MOVEMENT);
	}
}
