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