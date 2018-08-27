using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameType
{
	LocalMultiplayer,
	Multiplayer,
	WithAI,
	AIvsAI
}

public class Synchronisator : MonoBehaviour 
{
	static public Synchronisator Instance;

	public Color shipColor1;
	public Color shipColor2;
	public GameType gameType;
	public string shipName1;
	public string shipName2;

	public bool IsShipLocked(int index)
	{
		if (index == 0)
		{
			PlayerPrefs.SetInt(string.Format("Ship{0}Unlocked", index), 1);
		}
		return PlayerPrefs.GetInt(string.Format("Ship{0}Unlocked", index), 0) == 0 ? true : false;
	}

	public void UnlockShip(int index)
	{
		PlayerPrefs.SetInt(string.Format("Ship{0}Unlocked", index), 1);
	}

	void Awake()
	{
		if (Synchronisator.Instance != null)
		{
			Destroy(this.gameObject);
			return;
		}

		Instance = this;
	}

}