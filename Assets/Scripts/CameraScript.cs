using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour 
{
	private GameObject Player;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (GameObject.FindGameObjectsWithTag ("Player").Length > 0) {
			Player = GameObject.FindGameObjectsWithTag ("Player") [0];
			if (Player != null) {
				float playerScale = Player.transform.localScale.x;
				float playerX = Player.transform.position.x;
				float playerY = Player.transform.position.y;
				float cameraX = playerX;
				float cameraY = playerY;

				if(playerX > -6.5 || playerX < 6.5){
					cameraX = playerX;
				}
				if(playerY > -4.5 || playerY < 4.5){
					cameraY = playerY;
				}

				Camera.main.transform.position = new Vector3 (cameraX, cameraY, -10);

				if(playerScale < .1) {
					Camera.main.orthographicSize = playerScale * 30;
				}
			}
		}
	}
}