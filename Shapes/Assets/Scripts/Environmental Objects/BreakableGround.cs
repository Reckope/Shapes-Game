using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BreakableGround : MonoBehaviour
{

	private AudioSource AudioSource;
	// Start is called before the first frame update
	void Start()
	{
		AudioSource = GetComponent<AudioSource>();
		Assert.IsNotNull(AudioSource);
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
		}
	}

	private IEnumerator BreakGround()
	{
		AudioSource.Play();
		transform.position = new Vector2(999f, 999f);
		yield return new WaitForSeconds(5);
		Destroy(this.gameObject);
	}
}
