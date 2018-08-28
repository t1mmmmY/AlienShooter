using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPart : MonoBehaviour 
{
	[SerializeField] bool undestructible = false;

	public IShip shipController { get; private set; }

	void Start() 
	{
		shipController = GetComponentInParent<IShip>();

		if (shipController == null)
		{
			Debug.LogWarning("shipController == null");
		}
	}

	public void SetColor(Material material)
	{
		MeshRenderer meshRenderer = GetComponent<MeshRenderer>();

		if (meshRenderer != null)
		{
			meshRenderer.sharedMaterial = material;
		}
	}

	public void SetUndestructible(bool undestructible)
	{
		this.undestructible = undestructible;
	}

	public void Hit(bool forceDestroy = false)
	{
//		Debug.Log(this.gameObject.name + " Destroy");
		shipController.Hit(this.transform.position);
		if (!undestructible || forceDestroy)
		{
			this.Destroy();
		}
	}

	void Destroy()
	{
		Destroy(this.gameObject);
	}

}
