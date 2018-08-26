using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorElements : MonoBehaviour 
{
	[SerializeField] Text[] texts;
	[SerializeField] Image[] images;

	void Start()
	{
		SetColor(Synchronisator.Instance.shipColor1);
	}

	public void SetColor(Color color)
	{
		foreach (Text text in texts)
		{
			if (text != null)
			{
				text.color = color;
			}
		}

		foreach (Image image in images)
		{
			if (image != null)
			{
				image.color = color;
			}
		}
	}
}
