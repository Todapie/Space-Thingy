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
			Debug.Log (bullet.damage + " | " + mass);
			if (mass == 1)
				bulletMove.speed /= 2.5f;
			else
				Destroy (other.gameObject);

			Space s = gameObject.AddComponent<Space>();

			s.food = food;

			if (mass == 1)
				return;

			if (mass-bullet.damage > 0) 
			{
				s.DisperseFood(transform.position.x, transform.position.y, bullet.damage);
				mass -= bullet.damage;
				transform.localScale = new Vector3( transform.localScale.x - (0.01f * bullet.damage), transform.localScale.y - (0.01f * bullet.damage), 1.1f);
			}
			else 
			{
				s.DisperseFood(transform.position.x, transform.position.y, mass);
				Destroy (transform.gameObject);
			}
		}

		if (other.name.Contains ("Food")) 
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
						FoodScript f2 = other.gameObject.GetComponent<FoodScript>();

						if (mass >= f2.mass)
							foodTransform.position = transform.position;
						else if( mass < f2.mass)
							foodTransform.position = f2.transform.position;

						var massOfThisObject = mass;
						
						var massOfCollider = f2.mass;
						f.mass = massOfThisObject + massOfCollider;


						Vector2 velocityOfThisObject = movement;
						
						Vector2 velocityOfCollider = f2.movement;

						float VX = ((massOfThisObject * velocityOfThisObject.x) + (massOfCollider * velocityOfCollider.x)) / (massOfCollider + massOfThisObject);
						float VY = ((massOfThisObject * velocityOfThisObject.y) + (massOfCollider * velocityOfCollider.y)) / (massOfCollider + massOfThisObject);

						Vector2 resultant = new Vector2(VX,VY);
						float theta = Mathf.Atan(VY/VX) * (180f / Mathf.PI);
						f.rotation = theta;

						theta = Mathf.Abs(Mathf.Tan(theta));
						
						float ratioY = 1;
						float ratioX = 1;
						
						if (theta > 1f) 
						{
							ratioY = theta / (theta + 1f);
							ratioX = 1f / (theta + 1f);
						}
						else 
						{
							ratioX = (1f / (1f + theta));
							ratioY = (theta / (1f + theta));
						}

						f.speed = resultant;
						f.Direction.x = ratioX;
						f.Direction.y = ratioY;

						f.transform.localScale = new Vector3 (f.mass * 0.02f, f.mass * 0.02f, 1f);

						s.collison = true;
						f.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);

						Destroy(gameObject);
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
}
