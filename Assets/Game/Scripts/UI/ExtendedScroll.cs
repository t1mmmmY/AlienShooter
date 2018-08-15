using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExtendedScroll : MonoBehaviour
{
	[SerializeField] ScrollRect scrollRect;

	public int numberOfItems = 2;

	public int currentNumber { get; private set; }

	void Start()
	{
		currentNumber = 0;
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
