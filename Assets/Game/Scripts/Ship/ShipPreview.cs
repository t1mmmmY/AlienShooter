using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPreview : ShipController 
{
	[SerializeField] Transform shipContent;

	public void CreateShipPreview(string shipName)
	{
		GameObject prefab = Resources.Load<GameObject>("Ships/" + shipName);
		shipMesh = GameObject.Instantiate<GameObject>(prefab, shipContent);
	}
}
