using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniqueShip : MonoBehaviour 
{
	[SerializeField] float _bulletSpeed = 25.0f;
	[SerializeField] float _timeBetweenBullets = 0.4f;
	[SerializeField] float _shipWidth = 12;

	[SerializeField] Transform[] _shootPoints;

	public float bulletSpeed
	{
		get { return _bulletSpeed; }
	}

	public float timeBetweenBullets
	{
		get { return _timeBetweenBullets; }
	}

	public float shipWidth
	{
		get { return _shipWidth; }
	}

	public Transform[] shootPoints
	{
		get { return _shootPoints; }
	}
}
