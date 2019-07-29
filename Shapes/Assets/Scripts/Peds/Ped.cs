using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ped : MonoBehaviour
{

	private string pedName;
	private string sound;
	private float acceleration;
	private float maxSpeed;

	// Ped Name
	public void setName(string newName)
	{
		pedName = newName;
	}

	public string getName()
	{
		return pedName; 
	}

	// Ped Sound
	public void setSound(string newSound)
	{
		sound = newSound;
	}

	public string getSound()
	{
		return sound; 
	}

	// Ped Movement Speed
	public void setAcceleration(float newAcceleration)
	{
		acceleration = newAcceleration;
	}

	public float getAcceleration()
	{
		return acceleration; 
	}

	// Ped Max Speed
	public void setMaxSpeed(float newMaxSpeed)
	{
		maxSpeed = newMaxSpeed;
	}

	public float getMaxSpeed()
	{
		return maxSpeed; 
	}
}
