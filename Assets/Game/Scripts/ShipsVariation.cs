using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsVariation : ScriptableObject 
{
	public Color[] shipColors;
	public string[] shipNames;

	public Color GetRandomColor(Color exclusion)
	{
		Color selectedColor = Color.white;
		do
		{
			selectedColor = shipColors[Random.Range(0, shipColors.Length)];
		} while (selectedColor == exclusion);
		return selectedColor;
//		return shipColors[Random.Range(0, shipColors.Length)];
	}

	public string GetRandomShip()
	{
		return shipNames[Random.Range(0, shipNames.Length)];
	}
}
