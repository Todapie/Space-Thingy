using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public Vector2 speed = new Vector2(15, 15);
	public Vector2 Direction;
	private Vector2 movement;
	
	// Update is called once per frame
	void Update () {
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);
	}

	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement;
	}
}
