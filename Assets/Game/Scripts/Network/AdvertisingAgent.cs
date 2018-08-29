using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TapjoyUnity;

public class AdvertisingAgent : MonoBehaviour 
{
	void Start () 
	{
		if (!Tapjoy.IsConnected) 
		{
			Tapjoy.Connect();
		}
	}

	void ShowVideo()
	{
	}
}
