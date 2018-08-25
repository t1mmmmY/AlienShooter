using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour 
{
	[SerializeField] MeshRenderer[] healtBar;

	[SerializeField] Material green;
	[SerializeField] Material red;
	[SerializeField] Material orange;

	int health = 20;
	float healthCoef = 0.5f;

	public void Init(int health)
	{
		this.health = health;
		healthCoef = healtBar.Length / health;
		foreach (MeshRenderer meshR in healtBar)
		{
			meshR.material = green;
		}
	}

	public void SetHealth(int currentHealth)
	{
		for (int i = 0; i < health; i++)
		{
			if (i == currentHealth)
			{
				healtBar[(int)(i * healthCoef)].material = orange;
			}
			else
			{
				healtBar[(int)(i * healthCoef)].material = i < currentHealth ? green : red;
			}
		}
	}
}
