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
	}

	public string GetRandomShip()
	{
		for (int i = 0; i < shipNames.Length; i++)
		{
			if (Synchronisator.Instance.IsShipLocked(i))
			{
				//First locked ship
				return shipNames[i];
			}
		}
		//All ships unlocked
		return shipNames[Random.Range(0, shipNames.Length)];
	}
}
