using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour 
{
	Rigidbody2D bulletRigidbody;
	public ShipController owner { get; private set; }

	[SerializeField] HitEffect hitEffectPrefab;
	bool used = false;

	public Vector2 position
	{
		get
		{
			return transform.position;
		}
	}

	void Awake()
	{
		bulletRigidbody = GetComponent<Rigidbody2D>();
	}

	public void Init(Vector2 speed, Material material, ShipController owner)
	{
		this.owner = owner;
		BulletsManager.AddBullet(this);
		bulletRigidbody.velocity = speed;

		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		if (meshRenderer != null)
		{
			meshRenderer.sharedMaterial = material;
		}
	}

	public void Destroy()
	{
		//Add some effect
		GameObject.Instantiate<HitEffect>(hitEffectPrefab, transform.position, Quaternion.identity);
		BulletsManager.RemoveBullet(this);

		Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
		if (used)
		{
			return;
		}

		Bullet otherBullet = other.GetComponent<Bullet>();
		if (otherBullet != null)
		{
			if (otherBullet.owner != this.owner)
			{
				otherBullet.Destroy();
				this.Destroy();
			}
			return;
		}

		ShipPart shipPart = other.GetComponent<ShipPart>();
		if (shipPart != null)
		{
			if (shipPart.shipController != this.owner)
			{
				shipPart.Hit();
				used = true;
				this.Destroy();
			}
			return;
		}
	}
}
