using UnityEngine;
using System.Collections;

public class FoodScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision collision) {
		Debug.Log("Hit");
		if (collision.collider.name.Contains ("Food")) {

			//Destroy (gameObject);
		}
	}
}
