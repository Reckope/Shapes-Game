using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level01 : Level
{
	public static event Action PlayLevel01Intro;

	// Start is called before the first frame update
	void Start()
	{
		if(PlayLevel01Intro != null)
		{
			PlayLevel01Intro();
		}
	}
}
