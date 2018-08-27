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
		for (int i = 0; i < shipsVariation.shipNames.Length; i++)
		{
			ShipPreview shipPreview = GameObject.Instantiate<ShipPreview>(shipPreviewPrefab, content);
			shipPreview.CreateShipPreview(shipsVariation.shipNames[i]);
			allShips.Add(shipPreview);

			if (Synchronisator.Instance.IsShipLocked(i))
			{
				shipPreview.ShipLocked();
				shipPreview.SetColor(Color.black);
			}
		}
	}

	public void SetColor(Color color)
	{
		for (int i = 0; i < allShips.Count; i++)
		{
			if (!Synchronisator.Instance.IsShipLocked(i))
			{
				allShips[i].SetColor(color);
			}
		}
	}

}
