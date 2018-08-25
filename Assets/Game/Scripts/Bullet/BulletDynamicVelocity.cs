using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDynamicVelocity : MonoBehaviour 
{
	[SerializeField] float extraForce = 10;
	[SerializeField] float changeVelocitySpeed = 10;

	float elapsedTime = 0;

	void Update()
	{
		elapsedTime += Time.deltaTime * changeVelocitySpeed;
		transform.Translate(Mathf.Sin(elapsedTime) * extraForce * Time.deltaTime, 0, 0);
	}
}
