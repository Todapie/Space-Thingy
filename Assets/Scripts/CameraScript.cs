using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	private GameObject Player;
	private float playerX;
	private float playerY;
	private float cameraX;
	private float cameraY;
	
	// Use this for initialization
	void Start () 
	{
		playerX = Player.transform.position.x;
		playerY = Player.transform.position.y;
		cameraX = playerX;
		cameraY = playerY;
	}
	
	// Update is called once per frame
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

				if(playerX > -5 && playerX < 5)
				{
					cameraX = playerX;
				}
				if(playerY > -4.5 && playerY < 4.5)
				{
					cameraY = playerY;
				}

				Camera.main.transform.position = new Vector3 (cameraX, cameraY, -10);

				if(playerScale < .1) 
				{
					Camera.main.orthographicSize = playerScale * 30;
				}
			}
		}
	}
}