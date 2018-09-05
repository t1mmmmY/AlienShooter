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

	[SerializeField] GameObject player1Selection;
	[SerializeField] GameObject player2Selection;

	[SerializeField] SelectShipButton[] selectShipButtons;
	[SerializeField] Text gameModeDescription;

	[SerializeField] GameObject soundOnButton;
	[SerializeField] GameObject soundOffButton;

	bool player1Ready = false;
	bool player2Ready = false;

	void Start()
	{
		Application.targetFrameRate = 30;
		player1Selection.SetActive(false);
		player2Selection.SetActive(false);

		SetSound(Synchronisator.Instance.soundOn);

		AdvertisingManager.Instance.ShowBanner(() =>
			{
				AdvertisingManager.Instance.HideBanner();
			});
	}

	public void SelectPvP()
	{
		Synchronisator.Instance.gameType = GameType.LocalMultiplayer;
		gameModeDescription.text = "VS Friend";
	}

	public void SelectAI()
	{
		Synchronisator.Instance.gameType = GameType.WithAI;
		gameModeDescription.text = "VS Bot";
	}

	public void SelectMultiplayer()
	{
		Synchronisator.Instance.gameType = GameType.Multiplayer;
		gameModeDescription.text = "Multiplayer";
	}

	public void GoToShipScreen()
	{
		player1Selection.SetActive(false);
		player2Selection.SetActive(false);
		player1Selection.SetActive(true);
		player2Selection.SetActive(Synchronisator.Instance.gameType == GameType.LocalMultiplayer);
		menuScroll.MoveTo(1);
		player1Ready = false;
		player2Ready = false;

		foreach (SelectShipButton selectButton in selectShipButtons)
		{
			selectButton.ResetButton();
		}
		shipScroll1.ResetButton();
		shipScroll2.ResetButton();

		if (Synchronisator.Instance.gameType == GameType.LocalMultiplayer)
		{
			AdvertisingManager.Instance.HideBanner();
		}
		else
		{
			AdvertisingManager.Instance.ShowBanner(() =>
				{
					AdvertisingManager.Instance.HideBanner();
				});
		}
	}

	public void GoToModesScreen()
	{
		menuScroll.MoveTo(0);
		AdvertisingManager.Instance.ShowBanner(() =>
			{
				AdvertisingManager.Instance.HideBanner();
			});
	}

	public void StartGame()
	{
		AdvertisingManager.Instance.HideBanner();

		switch (Synchronisator.Instance.gameType)
		{
			case GameType.WithAI:
				UnityEngine.SceneManagement.SceneManager.LoadScene(2);
				break;
			case GameType.LocalMultiplayer:
				if (player1Ready && player2Ready)
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(2);
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

		GoToShipScreen();
	}

	public int GetShipCount()
	{
		return shipsVariation.shipNames.Length;
	}

	public void SoundOn()
	{
		SetSound(true);
	}

	public void SoundOff()
	{
		SetSound(false);
	}

	private void SetSound(bool isEnable)
	{
		soundOnButton.SetActive(!isEnable);
		soundOffButton.SetActive(isEnable);
		Synchronisator.Instance.soundOn = isEnable;
		AudioListener.volume = isEnable ? 1 : 0;
	}
}
