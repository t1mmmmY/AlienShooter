using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalMultiplayerEndScreen : MonoBehaviour 
{
	bool player1Ready = false;
	bool player2Ready = false;

	public void Player1Ready()
	{
		player1Ready = true;
		if (player2Ready)
		{
			RestartGame();
		}
	}

	public void Player2Ready()
	{
		player2Ready = true;
		if (player1Ready)
		{
			RestartGame();
		}
	}

	void RestartGame()
	{
		GameManager.Instance.RestartGame();
	}
}
