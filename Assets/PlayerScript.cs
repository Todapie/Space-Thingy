using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public Vector2 speed = new Vector2(50, 50);
	private Vector2 movement;
	public float inputX = 0f;
	public float inputY = 0.0f;

	void Update() {
		if (Input.GetKeyDown(KeyCode.W))
			inputY += 1;
		if (Input.GetKeyDown(KeyCode.S))
			inputY -= 1;
		if (Input.GetKeyDown(KeyCode.A))
			inputX -= 1;
		if (Input.GetKeyDown(KeyCode.D))
			inputX += 1;
		
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
	}
	
	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}
