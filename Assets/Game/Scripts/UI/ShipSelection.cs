using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSelection : MonoBehaviour 
{
	[SerializeField] ShipPreview shipPreviewPrefab;
	[SerializeField] ShipsVariation shipsVariation;
	[SerializeField] Transform content;
	List<ShipPreview> allShips = new List<ShipPreview>();

	void Awake()
	{
		foreach (string shipName in shipsVariation.shipNames)
		{
			ShipPreview shipPreview = GameObject.Instantiate<ShipPreview>(shipPreviewPrefab, content);
			shipPreview.CreateShipPreview(shipName);
			allShips.Add(shipPreview);
		}
	}

	public void SetColor(Color color)
	{
		foreach (ShipPreview ship in allShips)
		{
			ship.SetColor(color);
		}
	}

}
