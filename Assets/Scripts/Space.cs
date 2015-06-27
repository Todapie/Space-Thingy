using UnityEngine;
using System.Collections;

public class Space : MonoBehaviour 
{
	public Transform food;
	public bool collison = false;
	private int designatedFoodLimit;
	
	void Start () 
	{
		CreateGiantAsteroid ();
	}

	public void SpawnAllFoodRandomly()
	{
		designatedFoodLimit = 300;
		if (GameObject.FindGameObjectsWithTag ("Food").Length == 0) 
		{
			for (int i = 0; i < designatedFoodLimit; i++)
			{
				var foodTransform = Instantiate(food) as Transform;
				foodTransform.position = new Vector3 (Random.Range (-450, 450), Random.Range (-280, 280), 5f);
				FoodScript f = foodTransform.GetComponent<FoodScript>();
				f.rotation = Random.Range(1, 360);
				f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
				f.mass = 1;
				f.readyNow = true;
			}
		}
	}

	public void CreateGiantAsteroid()
	{
		if (GameObject.FindGameObjectsWithTag ("Food").Length == 0) 
		{
			var foodTransform = Instantiate (food) as Transform;
			foodTransform.position = new Vector3 (Random.Range (-15f, 15f), Random.Range (-15f, 15f), 5f);
			FoodScript f = foodTransform.GetComponent<FoodScript> ();
			f.rotation = Random.Range (1, 360);
			f.speed = new Vector2 (Random.Range (-1f, 1f), Random.Range (-1f, 1f));
			f.mass = 500;
			f.readyNow = true;
			foodTransform.localScale = new Vector3 (((f.mass-1) * 0.02f) + 0.4f, ((f.mass-1) * 0.02f) + 0.4f, 1.1f);
		}
	}

	public void CreateFood()
	{
		if (GameObject.FindGameObjectsWithTag ("Food").Length < designatedFoodLimit) 
		{
			var foodTransform = Instantiate(food) as Transform;
			foodTransform.position = new Vector3 (Random.Range (-450, 450), Random.Range (-280, 280), 5f);
			FoodScript f = foodTransform.GetComponent<FoodScript>();
			f.rotation = Random.Range(1, 360);
			f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			f.mass = 1;
			f.readyNow = true;
		}
	}
	
	public void DisperseFood(float x, float y, int damage, int size)
	{
		FoodScript f = null;
		Transform foodTransform = null;
		int remainder = size%10;
		float radius = 0f;
		float X = 0;
		float Y = 0;

		if (size < 10) 
		{
			GetRemainder (X, Y, remainder);
		} 
		else 
		{
			for (int i = 0; i < 10; i++) 
			{
				foodTransform = Instantiate (food) as Transform;
				f = foodTransform.GetComponent<FoodScript> ();
				f.speed = new Vector2 (Random.Range (-10f, 10f), Random.Range (-10f, 10f));
				f.readyNow = false;
				f.mass = Mathf.FloorToInt (damage / 10f);
				f.rotation = Random.Range (1, 360);

				if (radius == 0f)
					radius = ((((size - 1) * 0.02f) + 0.4f) * 6.25f) / 2f;

				bool check = false;
				while (!check || Mathf.Sqrt(Mathf.Pow (Mathf.Abs(X-x), 2) + Mathf.Pow (Mathf.Abs(Y-y), 2)) < radius * 1.1f) {
					X = Random.Range (x - radius, x + radius);
					Y = Random.Range (y - radius, y + radius);
					check = true;
				}

				foodTransform.position = new Vector3 (X, Y, 5);
				foodTransform.localScale = new Vector3 (((f.mass - 1) * 0.02f) + 0.4f, ((f.mass - 1) * 0.02f) + 0.4f, 1.1f);
			}
			GetRemainder (X, Y, remainder);
		}
	}

	public void GetRemainder(float X, float Y, int remainder)
	{
		Debug.Log ("Remainder: " + remainder);
		for (int i = 0; i < remainder; i++) 
		{
			var foodTransform = Instantiate (food) as Transform;
			FoodScript f = foodTransform.GetComponent<FoodScript>();
			f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			f.readyNow = false;
			f.mass = 1;
			f.rotation = Random.Range(1, 360);
			foodTransform.position = new Vector3 (X,Y, 5);
			foodTransform.localScale = new Vector3 (((f.mass-1) * 0.02f) + 0.4f, ((f.mass-1) * 0.02f) + 0.4f, 1.1f);
		}
	}
	
	void Update () 
	{
	}
}