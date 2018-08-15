using System.Collections;
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

	List<ColorButton> allColors = new List<ColorButton>();
	Color selectedColor;

	void Start()
	{
		for (int i = 0; i < shipsVariation.shipColors.Length; i++)
		{
			ColorButton colorButton = GameObject.Instantiate<ColorButton>(colorButtonPrefab, content);
			colorButton.Init(this, shipsVariation.shipColors[i]);
			allColors.Add(colorButton);
		}

		SelectColor(allColors[0]);
	}

	public void SelectColor(ColorButton colorButton)
	{
		selectedColor = colorButton.color;
		foreach (ColorButton button in allColors)
		{
			button.Select(button == colorButton);
		}

		shipSelection.SetColor(selectedColor);

		if (shipId == 0)
		{
			Synchronisator.Instance.shipColor1 = selectedColor;
		}
		else if (shipId == 1)
		{
			Synchronisator.Instance.shipColor2 = selectedColor;
		}
	}
}
