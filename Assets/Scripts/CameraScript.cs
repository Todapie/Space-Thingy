using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	private GameObject Player;
	private float playerX;
	private float playerY;
	private float cameraX;
	private float cameraY;

	void Update () 
	{
		if (GameObject.FindGameObjectsWithTag ("Player").Length > 0) 
		{
			Player = GameObject.FindGameObjectsWithTag ("Player") [0];
			if (Player != null) 
			{
				float playerScale = Player.transform.localScale.x;
				playerX = Player.transform.position.x;
				playerY = Player.transform.position.y;

				if(playerX > -18 && playerX < 18)
				{
					cameraX = playerX;
				}
				if(playerY > -16.5 && playerY < 16.5)
				{
					cameraY = playerY;
				}

				Camera.main.transform.position = new Vector3 (cameraX, cameraY, -10);

				//if(playerScale < .1) 
				//{
					Camera.main.orthographicSize = playerScale * 30;
				//}
			}
		}
	}
}