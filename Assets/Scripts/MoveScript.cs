using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {

	public Vector2 speed = new Vector2(1f, 1f);
	public Vector2 Direction = new Vector2(0f, 0f);
	public float rotation;
	private Vector2 movement;
	
	// Update is called once per frame
	void Start () {
		var tempRot = rotation;
		var multiplier = 1;

		if (tempRot == 90f)
			Direction.y += (multiplier);
		else if(tempRot == 180f)
			Direction.x -= (multiplier);
		else if(tempRot == 0f || tempRot == 360f)
			Direction.x += (multiplier);
		
		if (tempRot != 90f || tempRot != 180f || tempRot != 0f || tempRot != 360f) {
			//Get ratio from angle after converting radians to degrees, absolute value to prevent negatives
			var ratio = Mathf.Abs(Mathf.Tan(tempRot * Mathf.PI / 180f));
			var y = 0f;
			var x = 0f;
			var domainX = 1;
			var domainY = 1;
			
			//Domains accounted for, multipliers set for later on when we change inputX and inputY
			//(-x,+y)|(+x,+y)
			//-------|-------
			//(-x,-y)|(+x,-y)
			//
			if (tempRot > 90f && tempRot < 180f) {
				domainX = -1;
			} else if (tempRot > 180f && tempRot < 270f) {
				domainX = -1;
				domainY = -1;
			} else if (tempRot > 270f && tempRot < 360f) {
				domainY = -1;
			}
			//If ratio of y/x is greater than 1, we have to use percentages to determine how we increment x and y
			if (ratio > 1f) {
				y = ratio / (ratio + 1f);
				x = 1f / (ratio + 1f);
			}
			else {
				x = (1f / (1f + ratio));
				y = (ratio / (1f + ratio));
			}
			
			Direction.x += (x * domainX);
			Direction.y += (y * domainY);
		}
	}

	void Update()
	{
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);
	}

	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement * 5f;
	}
}
