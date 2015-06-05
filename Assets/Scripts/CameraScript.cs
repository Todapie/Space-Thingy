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
		Player = GameObject.FindGameObjectsWithTag("Player")[0];

		if (Player != null) 
		{
			Camera.main.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, -10);
			
			Camera.main.orthographicSize = Player.transform.localScale.x * 50;
		}
	}
}
