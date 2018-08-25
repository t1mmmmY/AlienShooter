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
			shipPart.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	void OnEnable()
	{
		StartCoroutine("PreviewShootLoop");
		StartCoroutine("PreviewSpecialShootLoop");
	}

	void OnDisable()
	{
		StopCoroutine("PreviewShootLoop");
		StopCoroutine("PreviewSpecialShootLoop");
	}

	void ShootPreview()
	{
		foreach (Transform shootPoint in shootPoints)
		{
			if (bulletPrefab != null)
			{
				Bullet bullet = GameObject.Instantiate<Bullet>(bulletPrefab, shootPoint.position, shootPoint.rotation);
				bullet.transform.parent = this.transform;
				bullet.transform.localScale *= 2;
				bullet.Init(shootPoint.transform.up * bulletSpeed * 2, newMaterial, this);
			}
		}
	}

	void SpecialShootPreview()
	{
		foreach (Transform shootPoint in specialShootPoints)
		{
			if (specialBulletPrefab != null)
			{
				Bullet bullet = GameObject.Instantiate<Bullet>(specialBulletPrefab, shootPoint.position, shootPoint.rotation);
				bullet.transform.parent = this.transform;
				bullet.transform.localScale *= 2;
				bullet.Init(shootPoint.transform.up * specialBulletSpeed * 2, newMaterial, this);
			}
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

	IEnumerator PreviewSpecialShootLoop()
	{
		do
		{
			yield return new WaitForSeconds(timeBetweenSpecialBullets);
			SpecialShootPreview();
		} while (true);
	}
}
