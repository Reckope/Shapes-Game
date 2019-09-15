using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level04 : MonoBehaviour
{
	public static event Action PlayLevel04Intro;

	// Start is called before the first frame update
	void Start()
	{
		if(PlayLevel04Intro != null)
		{
			PlayLevel04Intro();
		}
	}
}
