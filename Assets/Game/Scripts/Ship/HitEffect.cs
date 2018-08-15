using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour 
{
	[SerializeField] float lifeTime = 1.5f;
	[SerializeField] ParticleSystem particles;

	void Start() 
	{
		StartCoroutine("InvokeDestroy");
	}

	public void Init(Color color)
	{
		particles.startColor = color;
//		particles.main.startColor = color;
	}

	IEnumerator InvokeDestroy()
	{
		yield return new WaitForSeconds(lifeTime);
		Destroy(this.gameObject);
	}
}
