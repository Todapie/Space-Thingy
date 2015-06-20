using UnityEngine;
using System.Collections;

public class Space : MonoBehaviour 
{
	public Transform food;
	public bool collison = false;

	// Use this for initialization
	void Start () 
	{
		if (GameObject.FindGameObjectsWithTag ("Food").Length == 0) 
		{
			for (int i = 0; i < 100; i++)
			{
				var foodTransform = Instantiate(food) as Transform;
				foodTransform.position = new Vector3 (Random.Range (-490, 490), Random.Range (-390, 390), 5f);
				FoodScript f = foodTransform.GetComponent<FoodScript>();
				f.rotation = Random.Range(1, 360);
				f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
				f.mass = 1;
			}
		}
	}

	public void CreateFood()
	{
		var foodTransform = Instantiate(food) as Transform;
		foodTransform.position = new Vector3 (Random.Range (-490, 490), Random.Range (-390, 390), 5f);
		FoodScript f = foodTransform.GetComponent<FoodScript>();
		f.rotation = Random.Range(1, 360);
		f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
		f.mass = 1;
	}
	
	public void DisperseFood(float x, float y, int damage)
	{
		for (int i = 0; i < damage; i++) 
		{
			float scale = 0.4f;
			var fs = Instantiate (food) as Transform;
			FoodScript f = fs.GetComponent<FoodScript>();
			f.speed = new Vector2(Random.Range(-10f, 10f), Random.Range(-10f, 10f));
			f.mass = 1;
			f.rotation = Random.Range(1, 360);
			float X = x / 1.05f;
			float X2 = x * 1.05f;
			float Y = y / 1.05f;
			float Y2 = y * 1.05f;
			fs.transform.position = new Vector3 (Random.Range (X, X2), Random.Range (Y, Y2), 5);
			fs.transform.localScale = new Vector3 (scale, scale, 5);
		}
	}

	// Update is called once per frame
	void Update () 
	{
	}
}