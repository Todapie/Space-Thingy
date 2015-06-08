using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {
	public GameObject playerPrefab;
	public string Name;
	private string stringToEdit = "";
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void SpawnPlayer(string name)
	{
		var player = Instantiate (playerPrefab);
		player.transform.position = new Vector3(Random.Range(-7f, 7f), Random.Range(-5f, 5f), 0f);
		PlayerScript PlayerProperties = player.GetComponent<PlayerScript>();
		PlayerProperties.Name = name;
	}

	public void OnGUI()
	{
		Vector3 tmpPos = Camera.main.WorldToScreenPoint (transform.position);
		GUI.Label (new Rect (tmpPos.x/1.5f, tmpPos.y-10, 100, 20), "Player name");
		stringToEdit = GUI.TextField (new Rect (tmpPos.x/1.5f + 110, tmpPos.y-10, 200, 20), stringToEdit, 25);

		if (GUI.Button(new Rect(tmpPos.x - 125, tmpPos.y + 50, 250, 100), "Spawn player") && stringToEdit.Trim().Length > 0) 
		{
			SpawnPlayer(stringToEdit.Trim());
			Destroy(GetComponent<MainMenuScript>());
		}
	}
}
