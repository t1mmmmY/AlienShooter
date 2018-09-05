using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EndGameScreenType
{
	WinAI,
	LooseAI,
	Player1Win,
	Player2Win,
	WinMultiplayer,
	LooseMultiplayer
}

public class GameManager : MonoBehaviour 
{
	public static GameManager Instance;

	[SerializeField] Transform cameraTransform;
	[SerializeField] BoxCollider2D movingArea1;
	[SerializeField] BoxCollider2D movingArea2;
	[SerializeField] Button quitButton;
	[SerializeField] Button smallQuitButton;
	[SerializeField] Button smallQuitButtonMiddle;
	[SerializeField] ShipsVariation shipsVariation;
	[SerializeField] GameObject lostConnectionScreen;
	[SerializeField] GameObject waitingPanel;
	[SerializeField] EndGameScreen[] endGameScreens;

	[SerializeField] GameObject showAdButton;
	[SerializeField] GameObject adButtonUsed;
	[SerializeField] GameObject shipUnlockedLabel;

	List<IShip> shipControllers;
	bool endScreenEnabled = false;

	int numberOfRestarts
	{
		get
		{
			return PlayerPrefs.GetInt("NumberOfRestarts", 0);
		}
		set
		{
			PlayerPrefs.SetInt("NumberOfRestarts", value);
		}
	}

	void Awake()
	{
		Instance = this;
		shipControllers = new List<IShip>();
	}

	void Start()
	{
		AudioListener.volume = Synchronisator.Instance.soundOn ? 1 : 0;
		quitButton.gameObject.SetActive(false);
		PrepareGame(Synchronisator.Instance.gameType);
		StartCoroutine("GameTimer");
	}

	void PrepareGame(GameType gameType)
	{
		switch (gameType)
		{
			case GameType.WithAI:
				ShowSmallQuitButton();
				CreateLocalPlayer(0, Synchronisator.Instance.shipName1, Synchronisator.Instance.shipColor1, false);
				Color enemyColor = shipsVariation.GetRandomColor(Synchronisator.Instance.shipColor1);
				CreateAI(1, shipsVariation.GetRandomShip(), enemyColor);
				break;
			case GameType.Multiplayer:
				ShowSmallQuitButton();
				CreateLocalPlayer(PhotonNetwork.player.ID - 1, Synchronisator.Instance.shipName1, Synchronisator.Instance.shipColor1, true);
				break;
			case GameType.LocalMultiplayer:
				ShowSmallQuitButtonMiddle();
				CreateLocalPlayer(0, Synchronisator.Instance.shipName1, Synchronisator.Instance.shipColor1, false);
				CreateLocalPlayer(1, Synchronisator.Instance.shipName2, Synchronisator.Instance.shipColor2, false);
				break;
			case GameType.AIvsAI:
				CreateAI(0, shipsVariation.GetRandomShip(), shipsVariation.GetRandomColor(Color.white));
				CreateAI(1, shipsVariation.GetRandomShip(), shipsVariation.GetRandomColor(Color.white));
				break;
		}

		StartGame();
	}

	public void StartGame()
	{
		foreach (IShip ship in shipControllers)
		{
			ship.StartGame();
		}
	}

	public void RestartGame()
	{
		numberOfRestarts++;
		if (numberOfRestarts >= AdvertisingManager.Instance.adFrequency && AdvertisingManager.Instance.isFullScreenBannerReady)
		{
			numberOfRestarts = 0;
			AdvertisingManager.Instance.ShowFullScreenAd(() =>
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(2);
				});
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(2);
		}
	}

	public void RestartMultiplayerGame()
	{
		HideAllEndScreens();
		lostConnectionScreen.SetActive(false);
		for (int i = 0; i < shipControllers.Count; i++)
		{
			RemoveShip(shipControllers[i]);
		}
		waitingPanel.SetActive(true);
		NetworkHelper.Instance.JoinRoom();
	}

	public void WatchAdAndUnlockShip()
	{
		AdvertisingManager.Instance.ShowVideo(() =>
			{
				showAdButton.SetActive(false);
				adButtonUsed.SetActive(true);
				numberOfRestarts = 0;

				if (shipControllers.Count > 1)
				{
					Synchronisator.Instance.UnlockShip(shipControllers[1].shipNumber);
				}
			});
	}

	public void MultPlayerReady()
	{
	}

//	public void NextEnemy(IShip oldShip)
//	{
////		Synchronisator.Instance.UnlockShip(oldShip.shipNumber);
//		if (shipControllers.Contains(oldShip))
//		{
//			shipControllers.Remove(oldShip);
//		}
//		IShip newShip = CreateAI(1, shipsVariation.GetRandomShip(), shipsVariation.GetRandomColor(Synchronisator.Instance.shipColor1));
//		newShip.StartGame();
//		shipControllers.Add(newShip);
//	}

	void RemoveShip(IShip ship)
	{
		if (shipControllers.Contains(ship))
		{
			shipControllers.Remove(ship);
			Destroy(ship.GetShipController().gameObject);
		}
	}

	public IShip GetEnemyShip(IShip currentShip)
	{
		foreach (IShip ship in shipControllers)
		{
			if (ship != currentShip)
			{
				return ship;
			}
		}
		return null;
	}

	IShip CreateLocalPlayer(int id, string shipName, Color shipColor, bool multiplayer)
	{
		if (multiplayer)
		{
			GameObject shipPrefab = PhotonNetwork.Instantiate("ShipBase/" + "SpaceShipMult", Vector3.zero, Quaternion.identity, 0);
			ShipController newShip = shipPrefab.GetComponent<ShipController>();
//			GameObject shipPrefab = PhotonNetwork.Instantiate("ShipBase/" + "SpaceShipMult", Vector3.zero, Quaternion.identity, 0);
//			NetworkController newShip = shipPrefab.GetComponent<NetworkController>();

			SetupShipPosition(id, shipName, shipColor, newShip);

			shipControllers.Add(newShip);

			if (id % 2 == 1)
			{
				cameraTransform.Rotate(0, 0, 180);
			}

			return newShip;
		}
		else
		{
			ShipController shipPrefab = Resources.Load<ShipController>("ShipBase/" + "SpaceShip");
			ShipController newShip = GameObject.Instantiate<ShipController>(shipPrefab);

			SetupShipPosition(id, shipName, shipColor, newShip);

			shipControllers.Add(newShip);

			return newShip;
		}
	}

	IShip CreateAI(int id, string shipName, Color shipColor)
	{
		ShipController shipPrefab = Resources.Load<ShipController>("ShipBase/" + "SpaceShipAI");
		ShipController newShip = GameObject.Instantiate<ShipController>(shipPrefab);

		SetupShipPosition(id, shipName, shipColor, newShip);

		shipControllers.Add(newShip);

		return newShip;
	}

	void SetupShipPosition(int id, string shipName, Color color, ShipController newShip)
	{
		if (id % 2 == 0)
		{
			newShip.Init(id, movingArea1, color, shipName);
			newShip.transform.position = movingArea1.bounds.center;
		}
		else
		{
			newShip.Init(id, movingArea2, color, shipName);
			newShip.transform.position = movingArea2.bounds.center;
			newShip.transform.rotation = Quaternion.Euler(0, 0, 180);
		}
	}

//	void SetupShipPosition(int id, string shipName, Color color, NetworkController newShip)
//	{
//		if (id % 2 == 0)
//		{
//			newShip.Init(id, movingArea1, color, shipName);
//			newShip.transform.position = movingArea1.bounds.center;
//		}
//		else
//		{
//			newShip.Init(id, movingArea2, color, shipName);
//			newShip.transform.position = movingArea2.bounds.center;
//			newShip.transform.rotation = Quaternion.Euler(0, 0, 180);
//		}
//	}

	public void ShowSmallQuitButton()
	{
		smallQuitButton.gameObject.SetActive(true);
	}

	public void ShowSmallQuitButtonMiddle()
	{
		smallQuitButtonMiddle.gameObject.SetActive(true);
	}

	public void ShowQuitButton()
	{
		quitButton.gameObject.SetActive(true);
	}

	public void ShowEndGameScreen(EndGameScreenType screenType, bool shipUnlocked = false)
	{
		if (endScreenEnabled)
		{
			return;
		}
		GetScreen(screenType).SetActive(true);
		endScreenEnabled = true;

		if (screenType == EndGameScreenType.LooseAI)
		{
			showAdButton.SetActive(AdvertisingManager.Instance.isVideoReady && shipControllers.Count > 1);
			adButtonUsed.SetActive(false);
		}

		shipUnlockedLabel.SetActive(shipUnlocked);
	}

	void ShowLostConnectionScreen()
	{
		HideAllEndScreens();
		lostConnectionScreen.SetActive(true);
		endScreenEnabled = true;
	}

	public void Quit()
	{
		if (Synchronisator.Instance.gameType == GameType.Multiplayer)
		{
			NetworkHelper.Instance.StopMatchmaking();
		}

		numberOfRestarts++;
		if (numberOfRestarts >= AdvertisingManager.Instance.adFrequency && AdvertisingManager.Instance.isFullScreenBannerReady)
		{
			numberOfRestarts = 0;
			AdvertisingManager.Instance.ShowFullScreenAd(() =>
				{
					UnityEngine.SceneManagement.SceneManager.LoadScene(1);
				});
		}
		else
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene(1);
		}
	}

	IEnumerator GameTimer()
	{
		do
		{
			yield return new WaitForSeconds(1);

			if (Synchronisator.Instance.gameType == GameType.Multiplayer)
			{
				if (PhotonNetwork.playerList.Length < 2 || Application.internetReachability == NetworkReachability.NotReachable)
				{
					if (!waitingPanel.activeSelf)
					{
						ShowLostConnectionScreen();
					}
				}
			}

		} while (true);
	}

	GameObject GetScreen(EndGameScreenType screenType)
	{
		foreach (EndGameScreen screen in endGameScreens)
		{
			if (screen.screenType == screenType)
			{
				return screen.screen;
			}
		}
		return null;
	}

	void HideAllEndScreens()
	{
		foreach (EndGameScreen screen in endGameScreens)
		{
			screen.screen.SetActive(false);
		}
	}
}

[System.Serializable]
public class EndGameScreen
{
	public EndGameScreenType screenType;
	public GameObject screen;
}