using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(AudioSource))]
public class BreakableGround : MonoBehaviour
{
	public static event Action BrokeGround;
	private AudioSource AudioSource;
	// Start is called before the first frame update
	void Start()
	{
		AudioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(AudioSource);
		this.gameObject.SetActive(true);
	}

	// Update is called once per frame
	void Update()
	{
		
	}

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

	private IEnumerator BreakGround()
	{
		AudioSource.volume = 0.8f;
		AudioSource.Play();
		transform.position = new Vector2(999f, 999f);
		yield return new WaitForSeconds(5);
		//Destroy(this.gameObject);
		this.gameObject.SetActive(false);
	}
}
