using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public Vector2 speed = new Vector2(1, 1);
	private Vector2 movement;
	public float inputX = 0.0f;
	public float inputY = 0.0f;
	public float inputRot = 0.0f;
	void Update() {
		if (inputRot < 0f)
			inputRot += 360f;
		if (inputRot > 360f)
			inputRot -= 360f;
		if (Input.GetKeyDown (KeyCode.W)) {
			//Prevent divide by zero error when getting ratio of y/x from angle when the angle is 90 (tan(90) = UNDEFINED)
			if (inputRot == 90f)
				inputY += .5f;
			else if(inputRot == 180f)
				inputX -= .5f;
			else if(inputRot == 0f || inputRot == 360f)
				inputX += .5f;

			if (inputRot != 90f || inputRot != 180f || inputRot != 0f || inputRot != 360f) {
				//Get ratio from angle after converting radians to degrees, absolute value to prevent negatives
				var ratio = Mathf.Abs(Mathf.Tan(inputRot * Mathf.PI / 180f));
				var y = 0f;
				var x = 0f;
				var domainX = 1;
				var domainY = 1;
				
				//Domains accounted for, multipliers set for later on when we change inputX and inputY
				//(-x,+y)|(+x,+y)
				//---------------
				//(-x,-y)|(+x,-y)
				//
				if (inputRot > 90f && inputRot < 180f) {
					domainX = -1;
				} else if (inputRot > 180f && inputRot < 270f) {
					domainX = -1;
					domainY = -1;
				} else if (inputRot > 270f && inputRot < 360f) {
					domainY = -1;
				}
				//If ratio of y/x is greater than 1, we have to use percentages to determine how we increment x and y
				if ((float)ratio > 1f) {
					y = ratio / (ratio + 1f);
					x = 1f / (ratio + 1f);
				}
				else {
					x = (1f / (1f + ratio)) * 0.5f;
					y = (ratio / (1f + ratio)) * 0.5f;
				}
				
				inputX+= (x * domainX);
				inputY += (y * domainY);
			}
		}
		if (Input.GetKeyDown (KeyCode.S)) {
			//Prevent divide by zero error when getting ratio of y/x from angle when the angle is 90 (tan(90) = UNDEFINED)
			if (inputRot == 90f)
				inputY -= .5f;
			else if(inputRot == 180f)
				inputX += .5f;
			else if(inputRot == 0f || inputRot == 360f)
				inputX -= .5f;
						
			if (inputRot != 90f || inputRot != 180f || inputRot != 0f || inputRot != 360f) {
				//Get ratio from angle after converting radians to degrees, absolute value to prevent negatives
				var ratio = Mathf.Abs(Mathf.Tan(inputRot * Mathf.PI / 180f));
				var y = 0f;
				var x = 0f;
				var domainX = 1;
				var domainY = 1;
				
				//Domains accounted for, multipliers set for later on when we change inputX and inputY
				//(-x,+y)|(+x,+y)
				//---------------
				//(-x,-y)|(+x,-y)
				//
				if (inputRot > 90f && inputRot < 180f) {
					domainX = 1;
				} else if (inputRot > 180f && inputRot < 270f) {
					domainX = 1;
					domainY = 1;
				} else if (inputRot > 270f && inputRot < 360f) {
					domainY = 1;
				}
				//If ratio of y/x is greater than 1, we have to use percentages to determine how we increment x and y
				if ((float)ratio > 1f) {
					y = ratio / (ratio + 1f);
					x = 1f / (ratio + 1f);
				}
				else {
					x = (1f / (1f + ratio)) * 0.5f;
					y = (ratio / (1f + ratio)) * 0.5f;
				}
				
				inputX -= (x * domainX);
				inputY -= (y * domainY);
			}
		}
//		if (Input.GetKey(KeyCode.W) && inputY < 2)
//			//inputY += .1f;
//		if (Input.GetKeyDown(KeyCode.S))
//			inputY -= .5f;
//		if (Input.GetKey(KeyCode.S) && inputY > -2)
//			inputY -= .1f;
		if (Input.GetKey(KeyCode.A))
			inputRot += 1f;
		if (Input.GetKey(KeyCode.D))
			inputRot -= 1f;
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
