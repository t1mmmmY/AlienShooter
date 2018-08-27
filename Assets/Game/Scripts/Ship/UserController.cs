using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserController : ShipController 
{
	[SerializeField] float shipSpeed = 2.0f;

	protected override void Start()
	{
		base.Start();
	}

	void Update () 
	{
		Touch[] touches = Input.touches;

		if (Synchronisator.Instance.gameType != GameType.Multiplayer)
		{
			foreach (Touch touch in touches)
			{
				if (playerNumber % 2 == 0)
				{
					if (touch.position.y < Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
						touchPosition.y += 20;
						float x = touchPosition.x - position.x;
						float y = touchPosition.y - position.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
				else
				{
					if (touch.position.y > Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
						touchPosition.y -= 20;
						float x = position.x - touchPosition.x;
						float y = position.y - touchPosition.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
			}

			#if UNITY_EDITOR || UNITY_STANDALONE

			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			Move(new Vector2(horizontal, vertical) * shipSpeed);

			if (Input.GetMouseButton(0))
			{
				if (playerNumber % 2 == 0)
				{
					if (Input.mousePosition.y < Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						touchPosition.y += 20;
						float x = touchPosition.x - position.x;
						float y = touchPosition.y - position.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
				else
				{
					if (Input.mousePosition.y > Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						touchPosition.y -= 20;
						float x = position.x - touchPosition.x;
						float y = position.y - touchPosition.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
			}

			#endif

		}
		else
		{
			foreach (Touch touch in touches)
			{
				if (playerNumber % 2 == 0)
				{
					if (touch.position.y < Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
						touchPosition.y += 20;
						float x = touchPosition.x - position.x;
						float y = touchPosition.y - position.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
				else
				{
					if (touch.position.y < Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
						touchPosition.y -= 20;
						float x = position.x - touchPosition.x;
						float y = position.y - touchPosition.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
			}
	
			#if UNITY_EDITOR || UNITY_STANDALONE
	
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			Move(new Vector2(horizontal, vertical));
	
			if (Input.GetMouseButton(0))
			{
				if (playerNumber % 2 == 0)
				{
					if (Input.mousePosition.y < Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						touchPosition.y += 20;
						float x = touchPosition.x - position.x;
						float y = touchPosition.y - position.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
				else
				{
					if (Input.mousePosition.y < Screen.height / 2)
					{
						Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
						touchPosition.y -= 20;
						float x = position.x - touchPosition.x;
						float y = position.y - touchPosition.y;
						Vector3 movement = Vector3.ClampMagnitude(new Vector3(x, y, 0), 1.0f * shipSpeed);
						Move(movement);
					}
				}
			}
	
			#endif
		}

	}
}
