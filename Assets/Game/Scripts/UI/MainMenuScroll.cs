using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScroll : MonoBehaviour 
{
//	[SerializeField] int numberOfScreens = 2;
	[SerializeField] float screenHeight = 720;
	[SerializeField] float speed = 2;
	int currentScreenNumber = 0;

	RectTransform rectTransform;

	void Awake()
	{
		rectTransform = GetComponent<RectTransform>();
	}

	public void MoveTo(int screenNumber)
	{
		StopCoroutine("MoveToCoroutine");
		StartCoroutine("MoveToCoroutine", screenNumber);
	}

	IEnumerator MoveToCoroutine(int number)
	{
		float elapsedTime = 0;
		Vector2 startPosition = rectTransform.anchoredPosition;
		Vector2 position = startPosition;
		Vector2 endPosition = new Vector2(startPosition.x, number * screenHeight);

		do
		{
			yield return new WaitForEndOfFrame();
			elapsedTime += Time.deltaTime * speed;
			if (elapsedTime > 1)
			{
				elapsedTime = 1;
			}

			position = Vector2.Lerp(startPosition, endPosition, elapsedTime);
			rectTransform.anchoredPosition = position;

		} while (elapsedTime < 1);
	}
}
