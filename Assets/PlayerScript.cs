using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public Vector2 speed = new Vector2(50, 50);
	private Vector2 movement;
	public float inputX = 0.0f;
	public float inputY = 0.0f;
	public float inputRot = 0.0f;
	void Update() {
		if (Input.GetKeyDown(KeyCode.W))
			inputY += .5f;
		if (Input.GetKey(KeyCode.W) && inputY < 2)
			inputY += .1f;
		if (Input.GetKeyDown(KeyCode.S))
			inputY -= .5f;
		if (Input.GetKey(KeyCode.S) && inputY > -2)
			inputY -= .1f;
		if (Input.GetKey(KeyCode.A))
			inputRot += 1f;
		movement = new Vector2(
			speed.x * inputX,
			speed.y * inputY);
	}

	
	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement;
		GetComponent<Rigidbody2D> ().rotation = inputRot;
	}
}
