using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : ShipController 
{
	[SerializeField] float dangerDistance = 50;
	[SerializeField] float accelerationSpeed = 5.0f;
	[SerializeField] float xBulletSpeed = 5.0f;
	[SerializeField] bool revert = false;

	Vector2 oldMoving = Vector2.zero;

	protected override void Start()
	{
		base.Start();
	}

	protected override void EndGame()
	{
//		GameManager.Instance.NextEnemy();
//		base.EndGame();
	}

	void LateUpdate() 
	{
		List<Bullet> enemyBullets = BulletsManager.GetEnemyBullets(this);
		
		float vertical = GetVerticalMovement(enemyBullets);
		float horizontal = GetHorizontalMovement(enemyBullets);

		if (revert)
		{
			horizontal = -horizontal;
		}

		Vector2 moving = Vector2.Lerp(oldMoving, new Vector2(horizontal, vertical), Time.deltaTime * accelerationSpeed);
		oldMoving = moving;

		Move(moving);
	}

	float GetVerticalMovement(List<Bullet> enemyBullets)
	{
		//Find forward bullets
		List<Vector2> forwardBullets = new List<Vector2>();

		foreach (Bullet bullet in enemyBullets)
		{
			Vector2 bulletPos = bullet.position + bullet.bulletRigidbody.velocity * xBulletSpeed * Time.deltaTime;
//			Vector2 bulletPos = bullet.position;
//			if (bulletPos.x < position.x + shipWidth / 1.5f && bulletPos.x > position.x - shipWidth / 1.5f)
			if (bulletPos.x < position.x + shipWidth / 2 && bulletPos.x > position.x - shipWidth / 2)
			{
				forwardBullets.Add(bulletPos);
			}
		}

		//Find minimum distance
		float minDistance = dangerDistance * 2;
		foreach (Vector2 bulletPos in forwardBullets)
		{
			float distance = Vector2.Distance(new Vector2(bulletPos.x, position.y), bulletPos);
			if (distance < minDistance)
			{
				minDistance = distance;
			}
		}

		if (minDistance < dangerDistance)
		{
			return -Mathf.Clamp01(minDistance / dangerDistance);
		}
		else
		{
			return Mathf.Clamp01(minDistance / dangerDistance);
		}
	}

	float GetHorizontalMovement(List<Bullet> enemyBullets)
	{
		//Find safety point

		float minX = movingArea.bounds.min.x;
		float maxX = movingArea.bounds.max.x;

		Vector2 safeArea = Vector2.zero;

//		List<float> allPoints = new List<float>();
		List<Vector2> forwardBullets = new List<Vector2>();
		forwardBullets.Add(new Vector2(minX, position.y));
		forwardBullets.Add(new Vector2(maxX, position.y));
//		allPoints.Add(minX);
//		allPoints.Add(maxX);

		foreach (Bullet bullet in enemyBullets)
		{
			Vector2 bulletPos = bullet.position + bullet.bulletRigidbody.velocity * xBulletSpeed * Time.deltaTime;
			if (Vector2.Distance(position, bulletPos) < dangerDistance)
			{
				forwardBullets.Add(bulletPos);
			}
			else
			{
//				Debug.Log(Vector2.Distance(position, bulletPos).ToString());
			}
//			allPoints.Add(bullet.position.x + bullet.bulletRigidbody.velocity.x * xBulletSpeed * Time.deltaTime);
		}

		//Sort
		forwardBullets.Sort(new BulletComparer());
//		allPoints.Sort();

		float maxDistance = 0;
		//Find safe area with max distance
		for (int i = 0; i < forwardBullets.Count - 1; i++)
		{
			float distance = forwardBullets[i+1].x - forwardBullets[i].x;
			if (distance > maxDistance)
			{
				maxDistance = distance;

				if (distance > shipWidth)
				{
					safeArea = new Vector2(forwardBullets[i].x + shipWidth / 2.0f, forwardBullets[i+1].x - shipWidth / 2.0f);
				}
				else
				{
					float centralPoint = (forwardBullets[i].x + forwardBullets[i+1].x) / 2.0f;
					safeArea = new Vector2(centralPoint - shipWidth / 2.0f, centralPoint + shipWidth / 2.0f);
				}
			}
		}
//		for (int i = 0; i < allPoints.Count - 1; i++)
//		{
//			float distance = allPoints[i+1] - allPoints[i];
//			if (distance > maxDistance)
//			{
//				maxDistance = distance;
//
//				if (distance > shipWidth)
//				{
//					safeArea = new Vector2(allPoints[i] + shipWidth / 2.0f, allPoints[i+1] - shipWidth / 2.0f);
//				}
//				else
//				{
//					float centralPoint = (allPoints[i] + allPoints[i+1]) / 2.0f;
//					safeArea = new Vector2(centralPoint - shipWidth / 2.0f, centralPoint + shipWidth / 2.0f);
//				}
//			}
//		}


		float randomPos = Random.Range(safeArea.x, safeArea.y);

		return Mathf.Clamp(position.x - randomPos, -1.0f, 1.0f);
	}

	void OnDrawGizmos()
	{
		List<Bullet> enemyBullets = BulletsManager.GetEnemyBullets(this);
		foreach (Bullet bullet in enemyBullets)
		{
			Vector2 bulletPos = bullet.position + bullet.bulletRigidbody.velocity * xBulletSpeed * Time.deltaTime;
			Gizmos.DrawSphere(bulletPos, 1.0f);
		}
	}
}

public class BulletComparer : IComparer<Vector2>
{
	public int Compare(Vector2 v1, Vector2 v2)
	{
		return v1.x.CompareTo(v2.x);
	}
}