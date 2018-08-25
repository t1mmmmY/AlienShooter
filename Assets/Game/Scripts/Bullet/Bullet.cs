using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour 
{
	public Rigidbody2D bulletRigidbody { get; private set; }
	public ShipController owner { get; private set; }

	[SerializeField] HitEffect hitEffectPrefab;
	[SerializeField] MeshRenderer meshRenderer;
	bool used = false;

	public Vector2 speed { get; private set; }

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
		this.speed = speed;
		this.owner = owner;
		BulletsManager.AddBullet(this);
		bulletRigidbody.velocity = speed;

		if (meshRenderer != null)
		{
			meshRenderer.sharedMaterial = material;

			MeshRenderer[] childMeshes = GetComponentsInChildren<MeshRenderer>();
			foreach (MeshRenderer childMesh in childMeshes)
			{
				childMesh.sharedMaterial = material;
			}
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
