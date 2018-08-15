using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPreview : ShipController 
{
	[SerializeField] Transform shipContent;

	public void CreateShipPreview(string shipName)
	{
		UniqueShip prefab = Resources.Load<UniqueShip>("Ships/" + shipName);
		shipMesh = GameObject.Instantiate<UniqueShip>(prefab, shipContent);

		ShipPart[] shipParts = shipMesh.GetComponentsInChildren<ShipPart>();
		foreach (ShipPart shipPart in shipParts)
		{
			shipPart.SetUndestructible(true);
		}
	}

	void OnEnable()
	{
		StartCoroutine("PreviewShootLoop");
	}

	void OnDisable()
	{
		StopCoroutine("PreviewShootLoop");
	}

	void ShootPreview()
	{
		foreach (Transform shootPoint in shootPoints)
		{
			Bullet bullet = GameObject.Instantiate<Bullet>(bulletPrefab, shootPoint.position, shootPoint.rotation);
			bullet.transform.parent = this.transform;
			bullet.Init(shootPoint.transform.up * bulletSpeed * 2, newMaterial, this);
		}
	}

	IEnumerator PreviewShootLoop()
	{
		do
		{
			yield return new WaitForSeconds(timeBetweenBullets);
			ShootPreview();
		} while (true);
	}
}
