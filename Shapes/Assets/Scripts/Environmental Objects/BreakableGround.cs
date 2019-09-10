using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableGround : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag == Ped.States.Block.ToString())
		{
			BreakGround();
		}
	}

	private void BreakGround()
	{
		Destroy(this.gameObject);
	}
}
