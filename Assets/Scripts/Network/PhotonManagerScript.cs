using UnityEngine;
using System.Collections;

public class PhotonManagerScript : MonoBehaviour {
	const string VERSION = "v0.1";
	public string roomName = "Test Room";
	public string playerPrefab = "Player";

	// Use this for initialization
	void Start () 
	{
		PhotonNetwork.ConnectUsingSettings (VERSION);
	}
	void OnJoinedLobby()
	{	
		RoomOptions roomOptions = new RoomOptions() { isVisible = false, maxPlayers = 4 };
		PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, TypedLobby.Default);
	}
	void OnJoinedRoom()
	{
		PhotonNetwork.Instantiate (playerPrefab, new Vector3(Random.Range(-7f, 7f), Random.Range(-5f, 5f), 0f), Quaternion.identity, 0);
	}
}
