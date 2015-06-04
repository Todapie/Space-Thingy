using UnityEngine;
using System.Collections;

public class Space : MonoBehaviour {

	public Transform food;

	// Use this for initialization
	void Start () {
		var foodTransform = Instantiate(food) as Transform;
		
		foodTransform.position = new Vector3(Random.Range(-8,8), Random.Range(-5,5), 5);
	}
	
	// Update is called once per frame
	void Update () {

//		var foodTransform = Instantiate(food) as Transform;
//
//		foodTransform.position = new Vector3(Random.Range(1,1680), Random.Range(1,1258), 5);
//
//		Debug.Log ("DERPDEDERPHERPDERPDERP");

	}
}
