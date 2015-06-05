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
				foodTransform.position = new Vector3 (Random.Range (-8, 8), Random.Range (-5, 5), 5);
			}
		}

	}

	public void CreateFood() 
	{
		var foodTransform = Instantiate(food) as Transform;
		foodTransform.position = new Vector3 (Random.Range (-8, 8), Random.Range (-5, 5), 5);
	}

	// Update is called once per frame
	void Update () 
	{
	}
}