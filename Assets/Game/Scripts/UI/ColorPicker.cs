﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorPicker : MonoBehaviour 
{
	[Range(0, 1)]
	[SerializeField] int shipId = 0;
	[SerializeField] ColorButton colorButtonPrefab;
	[SerializeField] Transform content;
	[SerializeField] ShipsVariation shipsVariation;
	[SerializeField] ShipSelection shipSelection;
	[SerializeField] ColorElements colorElements;

	List<ColorButton> allColors = new List<ColorButton>();
	Color selectedColor;
	int colorNumber
	{
		get
		{
			return PlayerPrefs.GetInt("ColorNumber", 0);
		}
		set
		{
			PlayerPrefs.SetInt("ColorNumber", value);
		}
	}

	void Start()
	{
		for (int i = 0; i < shipsVariation.shipColors.Length; i++)
		{
			ColorButton colorButton = GameObject.Instantiate<ColorButton>(colorButtonPrefab, content);
			colorButton.Init(this, shipsVariation.shipColors[i]);
			allColors.Add(colorButton);
		}

		if (shipId == 0)
		{
			SelectColor(allColors[colorNumber]);
		}
		else
		{
			SelectColor(allColors[0]);
		}
	}

	public void SelectColor(ColorButton colorButton)
	{
		selectedColor = colorButton.color;

		if (shipId == 0)
		{
			Synchronisator.Instance.shipColor1 = selectedColor;
		}
		else if (shipId == 1)
		{
			Synchronisator.Instance.shipColor2 = selectedColor;
		}

		for (int i = 0; i < allColors.Count; i++)
		{
			allColors[i].Select(allColors[i] == colorButton);
			if (shipId == 0 && allColors[i] == colorButton)
			{
				colorNumber = i;
				colorElements.SetColor(selectedColor);
			}
		}

		shipSelection.SetColor(selectedColor);
	}
}
