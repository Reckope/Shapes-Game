using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Torch : MonoBehaviour
{
	private GameObject player;
	private bool attachToPlayer = false;

	// Start is called before the first frame update
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
		Assert.IsNotNull(player);
	}

	private void Update()
	{
		if(attachToPlayer)
		{
			transform.position = player.transform.position;
		}
	}

	private void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.layer == LayerMask.NameToLayer(Ped.PedType.Player.ToString()))
		{
			transform.localScale = new Vector3(0f, 0f, 0f);
			attachToPlayer = true;
		}
	}
}
