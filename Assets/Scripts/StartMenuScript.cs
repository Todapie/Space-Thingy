using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {
	
	Canvas StartMenu;
	
	void Start()
	{
		StartMenu = GetComponent<Canvas>();
		StartMenu.enabled = false;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Pause();
		}
	}
	
	public void Pause()
	{
		StartMenu.enabled = !StartMenu.enabled;
	}

}