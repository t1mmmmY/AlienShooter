using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPreview : ShipController 
{
	[SerializeField] Transform shipContent;

	Coroutine shootCoroutine;
	Coroutine specialShootCoroutine;

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

//	void ShootPreview()
//	{
//		foreach (Transform shootPoint in shootPoints)
//		{
//			if (bulletPrefab != null)
//			{
//				Bullet bullet = GameObject.Instantiate<Bullet>(bulletPrefab, shootPoint.position, shootPoint.rotation);
//				bullet.transform.parent = this.transform;
//				bullet.transform.localScale *= 2;
//				bullet.Init(shootPoint.transform.up * bulletSpeed * 2, newMaterial, this);
//			}
//		}
//	}
//
//	void SpecialShootPreview()
//	{
//		foreach (Transform shootPoint in specialShootPoints)
//		{
//			if (specialBulletPrefab != null)
//			{
//				Bullet bullet = GameObject.Instantiate<Bullet>(specialBulletPrefab, shootPoint.position, shootPoint.rotation);
//				bullet.transform.parent = this.transform;
//				bullet.transform.localScale *= 2;
//				bullet.Init(shootPoint.transform.up * specialBulletSpeed * 2, newMaterial, this);
//			}
//		}
//	}

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

//	IEnumerator PreviewShootLoop()
//	{
//		do
//		{
//			yield return new WaitForSeconds(timeBetweenBullets);
//			ShootPreview();
//		} while (true);
//	}
//
//	IEnumerator PreviewSpecialShootLoop()
//	{
//		do
//		{
//			yield return new WaitForSeconds(timeBetweenSpecialBullets);
//			SpecialShootPreview();
//		} while (true);
//	}
}
