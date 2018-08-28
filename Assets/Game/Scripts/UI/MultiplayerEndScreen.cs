using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplayerEndScreen : MonoBehaviour 
{
	[SerializeField] Text timerLabel;

	string timerText = "";
	int timeLeft = 5;

	void Start()
	{
		timerText = timerLabel.text;
		StartCoroutine("Timer");
	}

	IEnumerator Timer()
	{
		timerLabel.text = timerText.Replace("<X>", timeLeft.ToString());
		do
		{
			yield return new WaitForSeconds(1);
			timerLabel.text = timerText.Replace("<X>", timeLeft.ToString());
		} while (timeLeft > 0);

		GameManager.Instance.RestartMultiplayerGame();
	}

	public void PlayerReady()
	{
		GameManager.Instance.MultPlayerReady();
	}
}
