using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkillsUI : MonoBehaviour {
	
	Canvas canvas;
	
	void Start()
	{
		canvas = GetComponent<Canvas>();
		canvas.enabled = false;
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			Pause();
		}
	}
	
	public void Pause()
	{
		canvas.enabled = !canvas.enabled;
	}
}