using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode
{
	Burst,
	OneByOne
}

public class UniqueShip : MonoBehaviour 
{
	[SerializeField] FireMode _fireMode = FireMode.Burst;
	[SerializeField] FireMode _specialFireMode = FireMode.Burst;
	[SerializeField] float _bulletSpeed = 25.0f;
	[SerializeField] float _specialBulletSpeed = 25.0f;
	[SerializeField] float _timeBetweenBullets = 0.4f;
	[SerializeField] float _timeBetweenSpecialBullets = 1.0f;
	[SerializeField] float _shipWidth = 12;
	[SerializeField] int _health = 20;
	[SerializeField] Bullet _bulletPrefab;
	[SerializeField] Bullet _specialBulletPrefab;

	[SerializeField] Transform[] _shootPoints;
	[SerializeField] Transform[] _specialShootPoints;

	public FireMode fireMode
	{
		get { return _fireMode; }
	}

	public FireMode specialFireMode
	{
		get { return _specialFireMode; }
	}

	public float bulletSpeed
	{
		get { return _bulletSpeed; }
	}

	public float specialBulletSpeed
	{
		get { return _specialBulletSpeed; }
	}

	public float timeBetweenBullets
	{
		get { return _timeBetweenBullets; }
	}

	public float timeBetweenSpecialBullets
	{
		get { return _timeBetweenSpecialBullets; }
	}

	public float shipWidth
	{
		get { return _shipWidth; }
	}

	public int health
	{
		get { return _health; }
		set { _health = value; }
	}

	public Bullet bulletPrefab
	{
		get { return _bulletPrefab; }
	}

	public Bullet specialBulletPrefab
	{
		get { return _specialBulletPrefab; }
	}

	public Transform[] shootPoints
	{
		get { return _shootPoints; }
	}

	public Transform[] specialShootPoints
	{
		get { return _specialShootPoints; }
	}
}
