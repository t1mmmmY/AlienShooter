﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShip
{
	Vector2 position { get; }
	int playerNumber { get; set; }
	void StartGame();
	void Hit(Vector3 position);
}
