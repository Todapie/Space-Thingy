using UnityEngine;
using System.Collections;

public class Space : MonoBehaviour 
{
	public Transform food;
	public bool collison = false;
	
	void Start () 
	{
		if (GameObject.FindGameObjectsWithTag ("Food").Length == 0) 
		{
			for (int i = 0; i < 300; i++)
			{
				var foodTransform = Instantiate(food) as Transform;
				foodTransform.position = new Vector3 (Random.Range (-450, 450), Random.Range (-350, 350), 5f);
				FoodScript f = foodTransform.GetComponent<FoodScript>();
				f.rotation = Random.Range(1, 360);
				f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
				f.mass = 1;
				f.readyNow = true;
			}
		}
	}

	public void CreateFood()
	{
		if (GameObject.FindGameObjectsWithTag ("Food").Length < 300) 
		{
			var foodTransform = Instantiate(food) as Transform;
			foodTransform.position = new Vector3 (Random.Range (-450, 450), Random.Range (-350, 350), 5f);
			FoodScript f = foodTransform.GetComponent<FoodScript>();
			f.rotation = Random.Range(1, 360);
			f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			f.mass = 1;
			f.readyNow = true;
		}
	}
	
	public void DisperseFood(float x, float y, int damage)
	{
		for (int i = 0; i < damage; i++) 
		{
			var foodTransform = Instantiate (food) as Transform;
			FoodScript f = foodTransform.GetComponent<FoodScript>();
			f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			f.readyNow = false;
			f.mass = 1;
			f.rotation = Random.Range(1, 360);
			float X = x / 1.05f;
			float X2 = x * 1.05f;
			float Y = y / 1.05f;
			float Y2 = y * 1.05f;
			foodTransform.position = new Vector3 (Random.Range (X, X2), Random.Range (Y, Y2), 5);
			foodTransform.localScale = new Vector3(0.4f, 0.4f, 1.1f);
		}
	}

	// Update is called once per frame
	void Update () 
	{
	}
}