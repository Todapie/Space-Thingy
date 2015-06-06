/*using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {
	const string VERSION = "v0.1";
	public string roomName = "Test Room";
	public string playerPrefabname = "player";
	public Transform spawnPoint;

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
		PhotonNetwork.Instantiate (playerPrefabname, spawnPoint.position, spawnPoint.rotation, 0);
	}
}
*/