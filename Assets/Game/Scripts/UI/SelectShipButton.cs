using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectShipButton : MonoBehaviour 
{
	[Range(0, 1)]
	[SerializeField] int playerId = 0;
	[SerializeField] MainMenu mainMenu;
	[SerializeField] Button button;
	[SerializeField] Text buttonLabel;

	[SerializeField] Color normalColor = Color.green;
	[SerializeField] Color pressedColor = Color.yellow;

	public void ResetButton()
	{
		normalColor = Synchronisator.Instance.shipColor1;
		button.image.color = normalColor;
		buttonLabel.color = normalColor;
		buttonLabel.text = "START";
	}

	public void ShipSelected()
	{
		if (playerId == 0)
		{
			mainMenu.SelectShip1();
		}
		else if (playerId == 1)
		{
			mainMenu.SelectShip2();
		}

		button.image.color = pressedColor;
		buttonLabel.color = pressedColor;
		buttonLabel.text = "WAITING";
	}
}
