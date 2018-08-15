using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : Photon.MonoBehaviour, IPunObservable, IShip 
{
	[SerializeField] float bulletSpeed = 10.0f;
	[SerializeField] float timeBetweenBullets = 1.0f;
	[SerializeField] protected float shipWidth = 12;

	[SerializeField] protected BoxCollider2D movingArea;
	[SerializeField] protected Transform shootPoint;
	[SerializeField] BulletMult bulletPrefab;
	[SerializeField] HitEffect hitEffectPrefab;
	[SerializeField] HealthBar healthBar;

	[SerializeField] Material baseMaterial;
	[SerializeField] Color color;

	Material newMaterial;
	bool playingGame = false;
	int health = 10;
	string shipName = "";
	GameObject shipMesh;

	public int playerNumber { get; private set; }


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

//	[PunRPC]
	public void Init(int playerNumber, BoxCollider2D movingArea, Color color, string shipName)
	{
		this.playerNumber = playerNumber;
		this.movingArea = movingArea;
		this.shipName = shipName;

		this.color = color;

//		PhotonNetwork.RPC(photonView, "CreateShip", PhotonTargets.All, false);
		CreateShip();

		SetColor();

		photonView.RPC("Setup", PhotonTargets.Others, playerNumber, new Vector3(color.r, color.g, color.b), shipName);
	}

	[PunRPC]
	void Setup(int playerNumber, Vector3 color, string shipName)
	{
		this.playerNumber = playerNumber;
		this.movingArea = movingArea;
		this.shipName = shipName;

		this.color = new Color(color.x, color.y, color.z);

		CreateShip();
		SetColor();
	}

	void Update () 
	{
		Touch[] touches = Input.touches;

		foreach (Touch touch in touches)
		{
			if (playerNumber % 2 == 0)
			{
				if (touch.position.y < Screen.height / 2)
				{
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
					touchPosition.y += 20;
					float x = touchPosition.x - position.x;
					float y = touchPosition.y - position.y;
					Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f);
					Move(movement);
				}
			}
			else
			{
				if (touch.position.y < Screen.height / 2)
				{
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
					touchPosition.y -= 20;
					//					Debug.Log(touchPosition.ToString());
					float x = position.x - touchPosition.x;
					float y = position.y - touchPosition.y;
					Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f);
					Move(movement);
				}
//				if (touch.position.y > Screen.height / 2)
//				{
//					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
//					touchPosition.y -= 20;
//					float x = position.x -touchPosition.x;
//					float y = position.y - touchPosition.y;
//					Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f);
//					Move(movement);
//				}
			}
		}

		#if UNITY_EDITOR || UNITY_STANDALONE

		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Move(new Vector2(horizontal, vertical));

		if (Input.GetMouseButton(0))
		{
			if (playerNumber % 2 == 0)
			{
				if (Input.mousePosition.y < Screen.height / 2)
				{
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					touchPosition.y += 20;
					float x = touchPosition.x - position.x;
					float y = touchPosition.y - position.y;
					Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f);
					Move(movement);
				}
			}
			else
			{
				if (Input.mousePosition.y < Screen.height / 2)
				{
					Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
					touchPosition.y -= 20;
//					Debug.Log(touchPosition.ToString());
					float x = position.x - touchPosition.x;
					float y = position.y - touchPosition.y;
					Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f);
					Move(movement);
				}
			}
		}

		#endif

	}

//	[PunRPC]
	void CreateShip()
	{
		GameObject shipPrefab = Resources.Load<GameObject>("Ships/" + shipName);
		shipMesh = GameObject.Instantiate<GameObject>(shipPrefab, this.transform);
	}

	void SetColor()
	{
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

	protected void EndGame()
	{
		playingGame = false;
		GameManager.Instance.EndGame();
		//		UnityEngine.SceneManagement.SceneManager.LoadScene(0);
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
			photonView.RPC("Shoot", PhotonTargets.All);
//			Shoot();
		} while (playingGame);
	}

	[PunRPC]
	protected void Shoot()
	{
		BulletMult bullet = GameObject.Instantiate<BulletMult>(bulletPrefab, shootPoint.position, shootPoint.rotation);
		bullet.Init(shootPoint.transform.up * bulletSpeed, newMaterial, this);
	}

	public void Hit(Vector3 position)
	{
		photonView.RPC("HitRPC", PhotonTargets.All, position);

	}

	[PunRPC]
	private void HitRPC(Vector3 position)
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
		GameManager.Instance.ShowQuitButton();
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

	#region IPunObservable implementation

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			// We own this player: send the others our data
			stream.SendNext(this.health);
		}
		else
		{
			// Network player, receive data
			this.health = (int)stream.ReceiveNext();
		}
	}

	#endregion
}
