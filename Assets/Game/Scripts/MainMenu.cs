using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] GameObject modeSelectionPanel;
	[SerializeField] GameObject waitingPanel;
	[SerializeField] ShipsVariation shipsVariation;
	[SerializeField] ExtendedScroll shipScroll1;
	[SerializeField] ExtendedScroll shipScroll2;
	[SerializeField] MainMenuScroll menuScroll;

	bool player1Ready = false;
	bool player2Ready = false;

	public void SelectPvP()
	{
		Synchronisator.Instance.gameType = GameType.LocalMultiplayer;
	}

	public void SelectAI()
	{
		Synchronisator.Instance.gameType = GameType.WithAI;
	}

	public void SelectMultiplayer()
	{
		Synchronisator.Instance.gameType = GameType.Multiplayer;
	}

	public void GoToShipScreen()
	{
		menuScroll.MoveTo(1);
	}

	public void GoToModesScreen()
	{
		menuScroll.MoveTo(0);
	}

	public void StartGame()
	{
		switch (Synchronisator.Instance.gameType)
		{
			case GameType.WithAI:
				UnityEngine.SceneManagement.SceneManager.LoadScene(1);
				break;
			case GameType.LocalMultiplayer:
				if (player1Ready && player2Ready)
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(1);
				}
				break;
			case GameType.Multiplayer:
				modeSelectionPanel.SetActive(false);
				waitingPanel.SetActive(true);
				NetworkHelper.Instance.JoinRoom();
				break;
		}

	}

	public void SelectShip1()
	{
		int number = shipScroll1.currentNumber;
		Synchronisator.Instance.shipName1 = shipsVariation.shipNames[number];
		player1Ready = true;
		StartGame();
	}

	public void SelectShip2()
	{
		int number = shipScroll2.currentNumber;
		Synchronisator.Instance.shipName2 = shipsVariation.shipNames[number];
		player2Ready = true;
		StartGame();
	}

	public void StopMatchmaking()
	{
		NetworkHelper.Instance.StopMatchmaking();
		waitingPanel.SetActive(false);
		modeSelectionPanel.SetActive(true);
	}

}
