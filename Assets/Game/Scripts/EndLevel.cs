using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour 
{
	void OnTriggerEnter2D(Collider2D other) 
	{
		Bullet bullet = other.GetComponent<Bullet>();
		if (bullet != null)
		{
			BulletsManager.RemoveBullet(bullet);
		}

		Destroy(other.gameObject);
	}
}
