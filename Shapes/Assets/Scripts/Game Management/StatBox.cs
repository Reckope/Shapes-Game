using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class StatBox : MonoBehaviour
{
	public Text nameText;
	public Text valueText;

	public string Name { get; set; }
	public float Value { get; set; }

	// Start is called before the first frame update
	void Start()
	{
		nameText.text = Name;
		valueText.text = Value.ToString();
	}

	// Update is called once per frame
	void Update()
	{
		//Debug.Log(Name);
		//Debug.Log(Value);
	}
}
