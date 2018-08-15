using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour 
{
	[SerializeField] MeshRenderer[] healtBar;

	[SerializeField] Material green;
	[SerializeField] Material red;

	public void Init()
	{
		foreach (MeshRenderer meshR in healtBar)
		{
			meshR.material = green;
		}
	}

	public void SetHealth(int health)
	{
		for (int i = 0; i < healtBar.Length; i++)
		{
			healtBar[i].material = i < health ? green : red;
		}
	}
}
