using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestShipManager : MonoBehaviour 
{
	[SerializeField] BoxCollider2D movingArea1;

	void Start()
	{
		ShipController shipController = GameObject.FindObjectOfType<ShipController>();
		shipController.Init(0, movingArea1, Synchronisator.Instance.shipColor1, "");
		shipController.StartGame();
	}
}
