using UnityEngine;
using System.Collections;

public class Space : MonoBehaviour 
{
	public Transform food;

	// Use this for initialization
	void Start () 
	{
		if (GameObject.FindGameObjectsWithTag ("Food").Length == 0) {
			for (int i = 0; i < 20; i++)
			{
				var foodTransform = Instantiate(food) as Transform;
				foodTransform.position = new Vector3 (Random.Range (-7, 7), Random.Range (-4, 4), 5);
			}
		}
	}

	public void CreateFood() 
	{
		var foodTransform = Instantiate(food) as Transform;
		foodTransform.position = new Vector3 (Random.Range (-8, 8), Random.Range (-5, 5), 5);
	}

	public void DisperseFood(float x, float y, int damage)
	{
		for (int i = 0; i < damage; i++) 
		{
			var foodTransform = Instantiate (food) as Transform;
			float X = x / 1.01f;
			float X2 = x * 1.01f;
			float Y = y / 1.01f;
			float Y2 = y * 1.01f;
			foodTransform.position = new Vector3 (Random.Range (X, X2), Random.Range (Y,Y2), 5);
		}
	}

	// Update is called once per frame
	void Update () 
	{
	}
}