using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletsManager 
{
	private static List<Bullet> allBullets;

	static BulletsManager()
	{
		allBullets = new List<Bullet>();
	}

	public static void StartGame()
	{
		allBullets = new List<Bullet>();
	}

	public static void AddBullet(Bullet bullet)
	{
		allBullets.Add(bullet);
	}

	public static void RemoveBullet(Bullet bullet)
	{
		allBullets.Remove(bullet);
	}

	public static List<Bullet> GetEnemyBullets(ShipController player)
	{
		List<Bullet> enemyBullets = new List<Bullet>();

		foreach (Bullet bullet in allBullets)
		{
			if (bullet.owner.team != player.team)
			{
				enemyBullets.Add(bullet);
			}
		}

		return enemyBullets;
	}
}
