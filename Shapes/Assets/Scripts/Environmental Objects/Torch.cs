/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the torch on Level03.
* Attach this directly to the torch GameObject.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Torch : MonoBehaviour
{
	// GameObjects
	private GameObject player;

	// Global variables
	private bool attachToPlayer = false;
	private Vector3 attachedTorchScale;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag(Ped.PedType.Player.ToString());
		Assert.IsNotNull(player);
	}

	private void Start()
	{
		Assert.IsFalse(attachToPlayer);
		attachedTorchScale = new Vector3(0, 0, 0);
	}

	// Once the player collects the torch..
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer(Ped.PedType.Player.ToString()))
		{
			transform.localScale = attachedTorchScale;
			attachToPlayer = true;
		}
	}

	private void Update()
	{
		if(attachToPlayer)
		{
			transform.position = player.transform.position;
		}
	}
}
