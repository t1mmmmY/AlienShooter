using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Space : MonoBehaviour 
{
	[SerializeField] UserController userController;

	[SerializeField] Transform stars1;
	[SerializeField] Transform stars2;

	void Start()
	{
		DontDestroyOnLoad(this.gameObject);
	}

	void LateUpdate () 
	{
		if (userController != null)
		{
			Vector3 stars1Position = stars1.transform.position;
			Vector3 stars2Position = stars2.transform.position;

			stars1Position.x = -userController.transform.position.x / 10.0f;
			stars2Position.x = -userController.transform.position.x / 20.0f;

			stars1.transform.position = stars1Position;
			stars2.transform.position = stars2Position;
		}
	}
}
