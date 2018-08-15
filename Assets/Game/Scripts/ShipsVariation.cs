using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsVariation : ScriptableObject 
{
	public Color[] shipColors;
	public string[] shipNames;

	public Color GetRandomColor()
	{
		return shipColors[Random.Range(0, shipColors.Length)];
	}

	public string GetRandomShip()
	{
		return shipNames[Random.Range(0, shipNames.Length)];
	}
}
