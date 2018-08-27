using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPreview : ShipController 
{
	[SerializeField] Transform shipContent;

	Coroutine shootCoroutine;
	Coroutine specialShootCoroutine;
	bool shipLocked = false;

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

	public void ShipLocked()
	{
		shipLocked = true;
	}

	void OnEnable()
	{
		Bullet[] oldBullets = GetComponentsInChildren<Bullet>();
		for (int i = 0; i < oldBullets.Length; i++)
		{
			Destroy(oldBullets[i].gameObject);
		}

		shootCoroutine = StartCoroutine(PreviewShootLoop(false));
		specialShootCoroutine = StartCoroutine(PreviewShootLoop(true));
	}

	void OnDisable()
	{
		if (shootCoroutine != null)
		{
			StopCoroutine(shootCoroutine);
		}
		if (specialShootCoroutine != null)
		{
			StopCoroutine(specialShootCoroutine);
		}
	}

	protected void ShootPreview(Transform shootPoint, bool special)
	{
		if (shipLocked)
		{
			return;
		}

		Bullet prefab = special ? specialBulletPrefab : bulletPrefab;
		float speed = special ? specialBulletSpeed : bulletSpeed;

		if (prefab != null)
		{
			Bullet bullet = GameObject.Instantiate<Bullet>(prefab, shootPoint.position, shootPoint.rotation);
			bullet.transform.parent = this.transform;
			bullet.transform.localScale *= 2;
			bullet.Init(shootPoint.transform.up * speed * 2, newMaterial, this);
		}
	}


	IEnumerator PreviewShootLoop(bool special)
	{
		int shootPointNumber = -1;
		float time = special ? timeBetweenSpecialBullets : timeBetweenBullets;
		FireMode mode = special ? specialFireMode : fireMode;
		Transform[] points = special ? specialShootPoints : shootPoints;

		if (points == null)
		{
			yield break;
		}
		if (points.Length == 0)
		{
			yield break;
		}

		do
		{
			yield return new WaitForSeconds(time);
			switch (mode)
			{
				case FireMode.Burst:
					for (int i = 0; i < points.Length; i++)
					{
						ShootPreview(points[i], special);
					}
					break;
				case FireMode.OneByOne:
					shootPointNumber++;
					if (shootPointNumber >= points.Length)
					{
						shootPointNumber = 0;
					}

					ShootPreview(points[shootPointNumber], special);
					break;
			}
		} while (true);
	}

}
