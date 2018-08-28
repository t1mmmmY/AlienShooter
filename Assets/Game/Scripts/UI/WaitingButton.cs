using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingButton : MonoBehaviour 
{
	[SerializeField] Button button;
	[SerializeField] Image buttonImage;
	[SerializeField] Text label;
	[SerializeField] Color waitingColor;

	public void OnClick()
	{
		button.interactable = false;
		buttonImage.color = waitingColor;
		label.color = waitingColor;
		label.text = "WAITING";
	}
}
