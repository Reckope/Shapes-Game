/* 
* Author: Joe Davis
* Project: Shapes
* 2019
* Notes: 
* This is used to control the health potion.
* Attach this directly to the Health Potion Object. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class HealthPotion : MonoBehaviour
{
	// Global variables
	[SerializeField]
	private int livesGiven = 1;

	// =========================================================
	// MonoBehaviour Methods (In order of execution)
	// =========================================================

	private void Start()
	{
		Assert.AreNotEqual(livesGiven, 0);
	}

	// When the potion is collected..
	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer(Ped.PedType.Player.ToString()) 
		&& Player.Instance.Lives + livesGiven <= Player.Instance.MaxLives)
		{
			Player.Instance.IncreaseLives(livesGiven);
			this.gameObject.SetActive(false);
		}
	}
}
