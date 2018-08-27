using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtendedScroll : MonoBehaviour
{
	[SerializeField] ScrollRect scrollRect;
	[SerializeField] MainMenu mainMenu;
	[SerializeField] ColorElements colorElements;

	[SerializeField] Text lockedLabel;
	[SerializeField] Button playButton;
	[SerializeField] Image playButtonImage;
	[SerializeField] Text playButtonText;
	[SerializeField] Color disabledColor;
	[SerializeField] SelectShipButton selectShipButton;

	int numberOfItems = 2;
	int _currentNumber = 0;

	public int currentNumber 
	{ 
		get
		{
			return _currentNumber;
		}
		private set 
		{ 
			_currentNumber = value;
			SetLock();
		}
	}

	void Start()
	{
		currentNumber = 0;
		numberOfItems = mainMenu.GetShipCount();
		colorElements.onSetColor += OnSetColor;
	}

	public void ResetButton()
	{
		SetLock();
	}

//	void OnEnable()
//	{
//		SetLock();
//	}

	void SetLock()
	{
		if (Synchronisator.Instance == null)
		{
			return;
		}
		bool shipLocked = Synchronisator.Instance.IsShipLocked(_currentNumber);

		lockedLabel.gameObject.SetActive(shipLocked);
		playButton.interactable = !shipLocked;

		if (!selectShipButton.isShipSelected)
		{
			playButtonImage.color = shipLocked ? disabledColor : Synchronisator.Instance.shipColor1;
			playButtonText.color = shipLocked ? disabledColor : Synchronisator.Instance.shipColor1;
			playButtonText.text = shipLocked ? "LOCKED" : "START";
		}
	}

	void OnDestroy()
	{
		colorElements.onSetColor -= OnSetColor;
	}

	void OnSetColor(Color color)
	{
		bool shipLocked = Synchronisator.Instance.IsShipLocked(_currentNumber);
		playButtonImage.color = shipLocked ? disabledColor : Synchronisator.Instance.shipColor1;
		playButtonText.color = shipLocked ? disabledColor : Synchronisator.Instance.shipColor1;
	}

	public void OnValueChanged(Vector2 value)
	{
		if (Input.GetMouseButton(0))
		{
			StopCoroutine("MoveTo");
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (value.x < 0)
			{
				currentNumber = 0;
			}
			else if (value.x > 1)
			{
				currentNumber = numberOfItems - 1;
			}
			else
			{
				for (int i = 0; i < numberOfItems; i++)
				{
					if (value.x > i * 1.0f / numberOfItems && value.x < (i+1) * 1.0f / numberOfItems)
					{
						//This is the right position
						//0.0 1.0
						//0.0 0.5 1.0
						//0.0 0.33 0.67 1.0
						if (numberOfItems > 1)
						{
							StartCoroutine("MoveTo", (float)i / (numberOfItems-1));
						}
						else
						{
							StartCoroutine("MoveTo", 0);
						}

						currentNumber = i;
					}
				}
			}
		}
	}

	IEnumerator MoveTo(float position)
	{
		float elapsedTime = 0;
		do
		{
			yield return new WaitForEndOfFrame();

			elapsedTime += Time.deltaTime;
			scrollRect.horizontalNormalizedPosition = Mathf.Lerp(scrollRect.horizontalNormalizedPosition, position, elapsedTime);

		} while (scrollRect.horizontalNormalizedPosition != position);
	}
}
