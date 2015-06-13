using UnityEngine;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{
	public BulletScript bullet;
	public Vector2 speed = new Vector2(1, 1);
	public int size;
	private Vector2 movement;
	public float inputX = 0.0f;
	public float inputY = 0.0f;
	public float inputRot = 0.0f;
	private Vector3 origin = new Vector3 (0f, 0f, 0f);
	private Vector3 heading;
	private float headingAngle;
	private float ScaleThresholdCounter;
	public Transform Food;

	void Start() 
	{
		size = Mathf.RoundToInt((transform.localScale.x - 0.05f) / (0.01f));
		ScaleThresholdCounter = 0f;
	}

	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.name.Contains ("Bullet")) 
		{
			bullet = other.gameObject.GetComponent<BulletScript>();

			Destroy (other.gameObject);
			Space s = gameObject.AddComponent<Space>();
			s.food = Food;
			if (size-bullet.damage > 0) 
			{
				s.DisperseFood(transform.position.x, transform.position.y, bullet.damage);
				size -= bullet.damage;
				transform.localScale = new Vector3( transform.localScale.x - (0.01f * bullet.damage), transform.localScale.y - (0.01f * bullet.damage), 1.1f);
			}
			else 
			{
				s.DisperseFood(transform.position.x, transform.position.y, size);
				Destroy (transform.gameObject);
			}
		}
	}

	void findFood() 
	{
		float nearestFood = 9999999;
		Transform nearestFoodT = null;

		foreach(var obj in GameObject.FindGameObjectsWithTag("Food")) 
		{
			//Debug.Log("HIT");

			var objectPos = obj.transform.position;
			var distanceSqr = (objectPos - transform.position).sqrMagnitude;
			
			if (distanceSqr < nearestFood) 
			{
				nearestFoodT = obj.transform;
				nearestFood = distanceSqr;
			}
		}
		heading = nearestFoodT.position - transform.position;

		//Debug.Log (heading.x + " " + heading.y);

		//float d = Mathf.Abs( Mathf.Sqrt(heading.x + heading.y ) );
		float angleInRadian = Mathf.Atan2(heading.x, heading.y); //angle in radian
		headingAngle = angleInRadian * 180 / Mathf.PI;

		//Debug.Log (headingAngle);
	}
	
//	void OnCollisionEnter2D (Collision2D other) {
//		if (other.collider.name.Contains ("vertical")) {
//			if(inputX < 0)
//				inputX = 0.2f;
//			else
//				inputX = -0.2f;
//		}
//		if (other.collider.name.Contains ("horizontal")) {
//			if (inputY < 0)
//				inputY = 0.2f;
//			else
//				inputY = -0.2f;
//		}
//		if (other.collider.name.Contains ("Food")) {
//			Destroy (other.gameObject);
//			
//			Space s = gameObject.AddComponent<Space>();
//			//s.food = Food;
//			//s.CreateFood();
//			transform.localScale = new Vector3( transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 1.1f);
//		}
//	}
//	
	void Update() {

		ScaleThresholdCounter += 0.000003f;
		//Should be around 1 minute for your size to decrease
		
		if (ScaleThresholdCounter >= 0.01f) 
		{
			ScaleThresholdCounter = 0f;
			if (size > 1) 
			{
				transform.localScale = new Vector3 (transform.localScale.x - 0.01f, transform.localScale.y - 0.01f, 1.1f);
				size -= 1;
			}
		}

		//Debug.Log ("update");
		if (heading == origin) {
			//Debug.Log("1");
			//findFood ();
		} else if (headingAngle - inputRot >= -1 && headingAngle - inputRot <= 1) {
			//Debug.Log("2");
			inputRot = headingAngle;
		}
		else if (headingAngle != inputRot) {
			//Debug.Log("3");
			if (inputRot < 0f)
				inputRot += 360f;
			if (inputRot > 360f)
				inputRot -= 360f;
			if (headingAngle < 0) {
				inputRot -= 1f;
			} else {
				inputRot += 1f;
			}
		} else {
			//Debug.Log("BLARR");
			if (Mathf.Sqrt((inputX*inputX) + (inputY * inputY)) <= 2f) {
				//Debug.Log("Hit");
				Accelerate(true);
			}

			movement = new Vector2(
				speed.x * inputX,
				speed.y * inputY);
		}

	}
	
	void Accelerate(bool choice) {
		var tempRot = inputRot;
		var multiplier = 1;
		//Determine if W or S pressed. If S, flip rotation by 180 degrees so our math is opposite of W. 
		//Value stored in temp variable to prevent the ship from just rotating 180 degrees like a derp.
		if (!choice) {
			
			multiplier = -1;
			if (inputRot >= 180f)
				tempRot -= 180f;
			else if (inputRot == 0f)
				tempRot = 0f;
			else
				tempRot += 180f;
		}
		//Prevent divide by zero error when getting ratio of y/x from angle when the angle is 90 (tan(90) = UNDEFINED)
		if (tempRot == 90f)
			inputY += (.5f * multiplier);
		else if (tempRot == 180f)
			inputX -= (.5f * multiplier);
		else if (tempRot == 0f || tempRot == 360f) {
			inputX += (.5f * multiplier);
		}
		
		if (tempRot != 90f && tempRot != 180f && tempRot != 0f && tempRot != 360f) {
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
				x = (1f / (1f + ratio)) * 0.5f;
				y = (ratio / (1f + ratio)) * 0.5f;
			}
			
			inputX+= (x * domainX);
			inputY += (y * domainY);
		}
	}
	
	
//	void FixedUpdate()
//	{
//		// 5 - Move the game object
//		GetComponent<Rigidbody2D>().velocity = movement;
//		GetComponent<Rigidbody2D> ().rotation = inputRot;
//	}
}
