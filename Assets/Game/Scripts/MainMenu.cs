using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour 
{
	[SerializeField] GameObject modeSelectionPanel;
	[SerializeField] GameObject waitingPanel;
	[SerializeField] ShipsVariation shipsVariation;
	[SerializeField] ExtendedScroll shipScroll;
	[SerializeField] MainMenuScroll menuScroll;

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

	public void StartGame()
	{
		if (Synchronisator.Instance.gameType == GameType.Multiplayer)
		{
			modeSelectionPanel.SetActive(false);
			waitingPanel.SetActive(true);
			NetworkHelper.Instance.JoinRoom();
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}
	}

	public void SelectShip()
	{
		int number = shipScroll.currentNumber;
		Synchronisator.Instance.shipName1 = shipsVariation.shipNames[number];
		StartGame();
	}

	public void StopMatchmaking()
	{
		NetworkHelper.Instance.StopMatchmaking();
		waitingPanel.SetActive(false);
		modeSelectionPanel.SetActive(true);
	}

}
