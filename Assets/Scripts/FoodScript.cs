using UnityEngine;
using System.Collections;

public class FoodScript : MonoBehaviour 
{
	public Vector2 speed =  new Vector2 (0f, 0f);
	public Vector2 Direction = new Vector2(0f, 0f);
	public float rotation = 0f;
	public Transform food;
	public int mass = 1;
	private Vector2 movement = new Vector2(0f, 0f);
	public BulletScript bullet;
	public bool readyNow;
	private float beforeTimer = 0f;
	
	void Start () 
	{
		var tempRot = rotation;
		var multiplier = 1;
		
		if (tempRot == 90f)
			Direction.y += (multiplier);
		if (tempRot == 270f)
			Direction.y -= (multiplier);
		else if(tempRot == 180f)
			Direction.x -= (multiplier);
		else if(tempRot == 0f || tempRot == 360f)
			Direction.x += (multiplier);
		
		if (tempRot != 90f && tempRot != 180f && tempRot != 0f && tempRot != 360f && tempRot != 270f) 
		{
			//Get ratio from angle after converting radians to degrees, absolute value to prevent negatives
			var ratio = Mathf.Abs(Mathf.Tan(tempRot * Mathf.PI / 180f));
			var y = 0f;
			var x = 0f;
			var domainX = 1;
			var domainY = 1;
			
			if (tempRot > 90f && tempRot < 180f) 
			{
				domainX = -1;
			} else if (tempRot > 180f && tempRot < 270f) 
			{
				domainX = -1;
				domainY = -1;
			} else if (tempRot > 270f && tempRot < 360f) 
			{
				domainY = -1;
			}
			//If ratio of y/x is greater than 1, we have to use percentages to determine how we increment x and y
			if (ratio > 1f) 
			{
				y = ratio / (ratio + 1f);
				x = 1f / (ratio + 1f);
			}
			else 
			{
				x = (1f / (1f + ratio));
				y = (ratio / (1f + ratio));
			}
			
			Direction.x += (x * domainX);
			Direction.y += (y * domainY);
		}
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);
	}

	void Update() 
	{
		if(!readyNow)
		{
			float endTime = Time.time;
			if (endTime - beforeTimer >= 1.5f && beforeTimer != 0f) 
			{
				readyNow = true;
				beforeTimer = 0f;
			}
		}
	}
	
	void FixedUpdate()
	{
		movement = new Vector2(
			speed.x * Direction.x,
			speed.y * Direction.y);
	
		if(float.IsNaN(speed.x) || float.IsNaN(speed.y) || float.IsNaN(Direction.x) || float.IsNaN(Direction.y))
		{ 
			speed.x = 0f; speed.y = 0f; Direction.x = 0f; Direction.y = 0f; 
			movement = new Vector2(
				speed.x * Direction.x,
				speed.y * Direction.y);
		}
		GetComponent<Rigidbody2D>().velocity = movement;
	}

	void OnTriggerExit2D (Collider2D other)
	{
		FoodScript ff = other.gameObject.GetComponent<FoodScript>();
		if (ff == null)
			return;
		if (readyNow && ff.readyNow) 
		{
			foodToFoodCollision(other);
		}
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.name.Contains ("vertical")) 
		{
			Direction.x = Direction.x * -1;
		}

		if (other.name.Contains ("horizontal")) 
		{
			Direction.y = Direction.y * -1;
		}

		if (other.gameObject.name.Contains ("Bullet")) 
		{
			bullet = other.gameObject.GetComponent<BulletScript>();
			var bulletMove = other.gameObject.GetComponent<MoveScript>();

//			if (mass == 1)
//			{
//				bulletMove.speed /= 2.0f;
//				return;
//			}
			//else
				Destroy (other.gameObject);

			Space s = gameObject.AddComponent<Space>();

			s.food = food;
			readyNow = false;

			if (mass-bullet.damage >= 0) 
			{
				s.DisperseFood(transform.position.x, transform.position.y, bullet.damage, mass);
				mass -= bullet.damage;
				if (mass > 1)
					transform.localScale = new Vector3((mass * 0.02f) + 0.4f, (mass * 0.02f) + 0.4f, 1.1f);
				else
					transform.localScale = new Vector3(0.4f, 0.4f, 1.1f);
			}
			else 
			{
				s.DisperseFood(transform.position.x, transform.position.y, mass, mass);
				Destroy (transform.gameObject);
			}
		}

		if (other.name.Contains ("Food")) 
		{
			FoodScript ff = other.gameObject.GetComponent<FoodScript>();
			if (!readyNow || !ff.readyNow)
			{
				beforeTimer = Time.time;
			}
			else 
			{
				foodToFoodCollision(other);
			}
		}

	}
	void foodToFoodCollision(Collider2D other)
	{
		if (GameObject.FindGameObjectsWithTag ("Background").Length > 0) 
		{
			GameObject Background = GameObject.FindGameObjectsWithTag ("Background") [0];
			
			if (Background != null)
			{
				Space s = Background.transform.GetComponent<Space>();
				
				if(!s.collison)
				{
					var foodTransform = Instantiate(food) as Transform;
					FoodScript f = foodTransform.GetComponent<FoodScript>();
					f.readyNow = true;
					
					FoodScript f2 = other.gameObject.GetComponent<FoodScript>();
					
					if (mass >= f2.mass)
						foodTransform.position = transform.position;
					else if( mass < f2.mass)
						foodTransform.position = f2.transform.position;
					
					var massOfThisObject = mass;
					var massOfCollider = f2.mass;
					Vector2 velocityOfThisObject = speed;
					Vector2 velocityOfCollider = f2.speed;
					
					f.mass = massOfThisObject + massOfCollider;

//					float VX = ((massOfThisObject * velocityOfThisObject.x) + (massOfCollider * velocityOfCollider.x)) / (massOfCollider + massOfThisObject);
//					float VY = ((massOfThisObject * velocityOfThisObject.y) + (massOfCollider * velocityOfCollider.y)) / (massOfCollider + massOfThisObject);
					Destroy(gameObject);
//					if ((f.mass > 300 || f2.mass > 300) && (VX > 5f || VY > 5f))
//						Debug.Log ("[m1: " + massOfThisObject + ", v1: " + velocityOfThisObject + "] [m2: " + massOfCollider + ", v2: " + velocityOfCollider + "]");
//
//					
//					//prevent super fast horrifically awesome af food from bouncing around
//					while (Mathf.Sqrt(Mathf.Pow(VX, 2) + Mathf.Pow(VY, 2)) > 20f)
//					{
//						VX /= 1.001f;
//						VY /= 1.001f;
//					}
//
//					float theta = Mathf.Atan(VY/VX) * (180f / Mathf.PI);
//					f.rotation = theta;
//					
//					theta = Mathf.Abs(Mathf.Tan(theta));
//					

					
					//---------------------------------------------------------------------------------------------------------------------------------
					Vector2 resultant = ((massOfCollider * velocityOfCollider) + (massOfThisObject * velocityOfThisObject)) / (massOfCollider + massOfThisObject);

					if ((f.mass > 300 || f2.mass > 300) && (resultant.x > 4f || resultant.y > 4f))
						Debug.Log ("[m1: " + massOfThisObject + ", v1: " + velocityOfThisObject + ", d1: " + Direction.x +"] [m2: " + massOfCollider + ", v2: " + velocityOfCollider + ", d1: " + f2.Direction.x +"]");

					float angle = Mathf.Atan(resultant.y / resultant.x) * (180f / Mathf.PI);

					f.rotation = angle;
//					float velocity = massOfThisObject * velocityOfThisObject.x / (f.mass * (Mathf.Cos(angle) * (180f / Mathf.PI)));
//					float VX = velocity * (Mathf.Cos(angle) * (180f / Mathf.PI));
//					float VY = velocity * (Mathf.Sin(angle) * (180f / Mathf.PI));

					angle = (Mathf.Tan(angle));

					float ratioY = 1;
					float ratioX = 1;
					
					if (angle > 1f) 
					{
						ratioY = angle / (angle + 1f);
						ratioX = 1f / (angle + 1f);
					}
					else 
					{
						ratioX = (1f / (1f + angle));
						ratioY = (angle / (1f + angle));
					}



					f.speed = resultant;
					f.Direction.x = ratioX;
					f.Direction.y = ratioY;
					f.transform.localScale = new Vector3 ((f.mass * 0.02f) + 0.4f, (f.mass * 0.02f) + 0.4f, 1f);
					s.collison = true;
					f.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
				}
				else
				{
					Destroy(gameObject);
					s.collison = false;
				}
			}
		}
	}
}
