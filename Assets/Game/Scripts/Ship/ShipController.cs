using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour, IShip 
{
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

	[SerializeField] protected BoxCollider2D movingArea;
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
	[SerializeField] protected Bullet bulletPrefab;
	[SerializeField] HitEffect hitEffectPrefab;
	[SerializeField] HealthBar healthBar;

	[SerializeField] Material baseMaterial;
	[SerializeField] Color color;

	protected Material newMaterial;
	protected bool playingGame = false;
	int health = 10;
	protected UniqueShip shipMesh;

	public int playerNumber { get; private set; }

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

	protected virtual void Start()
	{
	}

	public void Init(int playerNumber, BoxCollider2D movingArea, Color color, string shipName)
	{
		this.playerNumber = playerNumber;
		this.movingArea = movingArea;

		CreateShip(shipName);
		SetColor(color);
	}

	void CreateShip(string shipName)
	{
		UniqueShip shipPrefab = Resources.Load<UniqueShip>("Ships/" + shipName);
		shipMesh = GameObject.Instantiate<UniqueShip>(shipPrefab, this.transform);
	}

	public void SetColor(Color color)
	{
		if (color != Color.black)
		{
			this.color = color;
		}

		newMaterial = new Material(baseMaterial);
		newMaterial.color = color;
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
		StartCoroutine("ShootLoop");
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

	IEnumerator ShootLoop()
	{
		do
		{
			yield return new WaitForSeconds(timeBetweenBullets);
			Shoot();
		} while (playingGame);
	}

	protected void Shoot()
	{
		foreach (Transform shootPoint in shootPoints)
		{
			Bullet bullet = GameObject.Instantiate<Bullet>(bulletPrefab, shootPoint.position, shootPoint.rotation);
			bullet.Init(shootPoint.transform.up * bulletSpeed, newMaterial, this);
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
			GameManager.Instance.NextEnemy();
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
