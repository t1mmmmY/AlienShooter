using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour 
{
	[SerializeField] Transform shipPosition;
	[SerializeField] Material shipMaterial;

	UniqueShip ship;

	void OnEnable()
	{
		UniqueShip prefab = Resources.Load<UniqueShip>("Ships/" + Synchronisator.Instance.shipName1);
		ship = GameObject.Instantiate<UniqueShip>(prefab, shipPosition);

		Material newMaterial = new Material(shipMaterial);
		newMaterial.color = Synchronisator.Instance.shipColor1;
		ShipPart[] allParts = ship.transform.GetComponentsInChildren<ShipPart>(true);
		foreach (ShipPart part in allParts)
		{
			part.SetColor(newMaterial);
		}
	}

	void OnDisable()
	{
		if (ship != null)
		{
			Destroy(ship.gameObject);
		}
	}
}
