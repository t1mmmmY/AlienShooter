using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoAimBullet : MonoBehaviour 
{
	[SerializeField] Vector3 point = new Vector3(0, 1, -1);
	[SerializeField] float speed = 25;
	[SerializeField] float rotationSpeed = 10;
	IShip owner;
	IShip enemy;
	Bullet bullet;
	Rigidbody2D rigid;
	bool invert = false;

	void Start()
	{
		bullet = GetComponent<Bullet>();
		owner = bullet.owner;
		rigid = GetComponent<Rigidbody2D>();

		if (owner.playerNumber % 2 == 1)
		{
			invert = true;
			point = new Vector3(0, -1, -1);
		}
	}

	void FixedUpdate()
	{
		if (enemy == null && GameManager.Instance != null)
		{
			enemy = GameManager.Instance.GetEnemyShip(owner);
		}

		if (enemy != null)
		{
			Vector3 enemyPos = new Vector3(enemy.position.x, 0, enemy.position.y);
			if (invert)
			{
				enemyPos.z = -enemyPos.z;
			}
			rigid.transform.LookAt(enemyPos, point);
			rigid.transform.localRotation = Quaternion.Euler(0, 0, rigid.transform.localRotation.eulerAngles.z);
			rigid.velocity = Vector2.Lerp(rigid.velocity, new Vector2(rigid.transform.up.x, rigid.transform.up.y) * speed, Time.deltaTime * rotationSpeed);
		}
	}
}
