using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour 
{
	[SerializeField] Image image;
	[SerializeField] Image selectionImage;

	public Color color { get; private set; }
	ColorPicker owner;

	public void Init(ColorPicker owner, Color color)
	{
		this.owner = owner;
		this.color = color;
		image.color = color;
	}

	public void Select(bool selected)
	{
		selectionImage.color = new Color(1.0f, 1.0f, 1.0f, selected ? 1.0f : 0.0f);
	}

	public void SelectColor()
	{
		owner.SelectColor(this);
	}
}
