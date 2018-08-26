using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IShip 
{
	[SerializeField] protected BoxCollider2D movingArea;
	[SerializeField] HitEffect hitEffectPrefab;
	[SerializeField] HealthBar healthBar;
	[SerializeField] Material baseMaterial;
	[SerializeField] Color color;

	protected Material newMaterial;
	protected bool playingGame = false;

	protected UniqueShip shipMesh;

	public int playerNumber { get; set; }

	protected FireMode fireMode
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.fireMode;
			}
			else
			{
				return FireMode.Burst;
			}
		}
	}

	protected FireMode specialFireMode
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.specialFireMode;
			}
			else
			{
				return FireMode.Burst;
			}
		}
	}

	protected float bulletSpeed
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.bulletSpeed;
			}
			else
			{
				return 25;
			}
		}
	}

	protected float specialBulletSpeed
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.specialBulletSpeed;
			}
			else
			{
				return 25;
			}
		}
	}

	protected float timeBetweenBullets
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.timeBetweenBullets;
			}
			else
			{
				return 0.4f;
			}
		}
	}

	protected float timeBetweenSpecialBullets
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.timeBetweenSpecialBullets;
			}
			else
			{
				return 1.0f;
			}
		}
	}

	protected float shipWidth
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.shipWidth;
			}
			else
			{
				return 12;
			}
		}
	}

	public int health
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.health;
			}
			else
			{
				return 20;
			}
		}
		set
		{
			if (shipMesh != null)
			{
				shipMesh.health = value;
			}
		}
	}

	public int team
	{
		get
		{
			return playerNumber % 2;
		}
	}

	public Vector2 position
	{
		get
		{
			return transform.position;
		}
	}

	protected Bullet bulletPrefab
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.bulletPrefab;
			}
			else
			{
				return null;
			}
		}
	}

	protected Bullet specialBulletPrefab
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.specialBulletPrefab;
			}
			else
			{
				return null;
			}
		}
	}

	protected Transform[] shootPoints
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.shootPoints;
			}
			else
			{
				return null;
			}
		}
	}

	protected Transform[] specialShootPoints
	{
		get
		{
			if (shipMesh != null)
			{
				return shipMesh.specialShootPoints;
			}
			else
			{
				return null;
			}
		}
	}

	protected virtual void Start()
	{
	}

	public void Init(int playerNumber, BoxCollider2D movingArea, Color color, string shipName)
	{
		this.playerNumber = playerNumber;
		this.movingArea = movingArea;

		if (shipName != "")
		{
			CreateShip(shipName);
		}
		else
		{
			InitShipMesh();
		}
		SetColor(color);
	}

	void CreateShip(string shipName)
	{
		UniqueShip shipPrefab = Resources.Load<UniqueShip>("Ships/" + shipName);
		shipMesh = GameObject.Instantiate<UniqueShip>(shipPrefab, this.transform);
	}

	void InitShipMesh()
	{
		shipMesh = GetComponentInChildren<UniqueShip>();
	}

	public void SetColor(Color color)
	{
		if (color != Color.black)
		{
			this.color = color;
		}

		newMaterial = new Material(baseMaterial);
		newMaterial.color = this.color;
		ShipPart[] allParts = GetComponentsInChildren<ShipPart>(true);
		foreach (ShipPart part in allParts)
		{
			part.SetColor(newMaterial);
		}
	}

	public void StartGame()
	{
		playingGame = true;
		BulletsManager.StartGame();
		StartCoroutine(ShootLoop(false));
		StartCoroutine(ShootLoop(true));
//		StartCoroutine("SpecialShootLoop");
	}

	protected virtual void EndGame()
	{
		playingGame = false;
		GameManager.Instance.EndGame();
	}

	protected void Move(Vector2 direction)
	{
		if (playingGame)
		{
			transform.Translate(direction);
			if (!movingArea.bounds.Contains(transform.position))
			{
				transform.position = movingArea.bounds.ClosestPoint(transform.position);
			}
		}
	}

	IEnumerator ShootLoop(bool special)
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
						Shoot(points[i], special);
					}
					break;
				case FireMode.OneByOne:
					shootPointNumber++;
					if (shootPointNumber >= points.Length)
					{
						shootPointNumber = 0;
					}

					Shoot(points[shootPointNumber], special);
					break;
			}
		} while (playingGame);
	}

	protected void Shoot(Transform shootPoint, bool special)
	{
		Bullet prefab = special ? specialBulletPrefab : bulletPrefab;
		float speed = special ? specialBulletSpeed : bulletSpeed;

		if (prefab != null)
		{
			Bullet bullet = GameObject.Instantiate<Bullet>(prefab, shootPoint.position, shootPoint.rotation);
			bullet.Init(shootPoint.transform.up * speed, newMaterial, this);
		}
	}

	public void Hit(Vector3 position)
	{
		HitEffect hitEffect = GameObject.Instantiate<HitEffect>(hitEffectPrefab, position, Quaternion.identity, this.transform);
		hitEffect.Init(color);

		if (health > 0)
		{
			health--;
			healthBar.SetHealth(health);

			if (health == 0)
			{
				Die();
			}
		}
	}


	protected void Die()
	{
		playingGame = false;
		StartCoroutine("DieEffect");
	}

	IEnumerator DieEffect()
	{
		if (Synchronisator.Instance.gameType != GameType.WithAI)
		{
			GameManager.Instance.ShowQuitButton();
		}
		else
		{
			if (playerNumber != 0)
			{
				GameManager.Instance.NextEnemy(this);
			}
		}

		ShipPart[] shipParts = GetComponentsInChildren<ShipPart>();
		healthBar.gameObject.SetActive(false);

 		for (int i = 0; i < shipParts.Length; i++)
		{
			if (shipParts[i] != null)
			{
				shipParts[i].Hit(true);
			}

			yield return new WaitForSeconds(0.025f);
		}

		yield return new WaitForSeconds(2.0f);

		EndGame();
	}

}
