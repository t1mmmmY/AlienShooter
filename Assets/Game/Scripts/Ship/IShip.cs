using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShip
{
	void StartGame();
	void Hit(Vector3 position);
}
