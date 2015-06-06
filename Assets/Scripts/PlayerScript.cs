using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
	public GameObject bullet;

	public Vector2 speed = new Vector2(1, 1);
	private Vector2 movement;
	public float inputX = 0.0f;
	public float inputY = 0.0f;
	public float inputRot = 0.0f;
	public Rigidbody2D rb;
	public Transform Food;
	public NetworkView nv;
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;
	[RPC]
	void PrintText(string text) {
		Debug.Log (text);
	}

	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;
		if (stream.isWriting)
		{
			syncPosition = rb.position;
			stream.Serialize(ref syncPosition);

			syncVelocity = rb.velocity;
			stream.Serialize(ref syncVelocity);
		}
		else
		{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);

			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;
			
			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = rb.position;
		}
	}

	void Start() 
	{
		//var player = Instantiate(Player) as Transform;
		transform.localScale = new Vector3( 0.05f, 0.05f, 1.0f);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.name.Contains ("Food") && transform.localScale.y <= 0.5f) 
		{
			Destroy (other.gameObject);
			Space s = gameObject.AddComponent<Space>();
			s.food = Food;
			s.CreateFood();
			transform.localScale = new Vector3( transform.localScale.x + 0.01f, transform.localScale.y + 0.01f, 1.1f);
		}
	}

	void OnCollisionEnter2D (Collision2D other) {
		if (other.collider.name.Contains ("vertical")) 
		{
			if(inputX < 0)
				inputX = 0.2f;
			else
				inputX = -0.2f;
		}
		if (other.collider.name.Contains ("horizontal")) 
		{
			if (inputY < 0)
				inputY = 0.2f;
			else
				inputY = -0.2f;
		}

	}

	void OnGUI()
	{
		Debug.Log (transform.position.x);
		GUI.Label(new Rect(transform.position.x,transform.position.y, 100, 50), "Size: " + Mathf.RoundToInt((transform.localScale.x - 0.05f) / 0.01f));

	}

	void Update() 
	{
		//Players half life ~ 190s or 3 minutes 10 seconds.
		if (transform.localScale.y > 0.05f)
			transform.localScale = new Vector3 (transform.localScale.x - 0.000001f, transform.localScale.y - 0.000001f, 1.1f);

		if (Input.GetKeyDown (KeyCode.H)) {
			nv.RPC ("PrintText", RPCMode.All, "Hello world");
		}

		if (nv.isMine) {
			if (inputRot < 0f)
				inputRot += 360f;
			if (inputRot > 360f)
				inputRot -= 360f;
			if (Input.GetKey (KeyCode.W)) {
				if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) <= 1.9f) {
					Accelerate (true);
				} else if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) >= 5f) {
					inputX /= 1.1f;
					inputY /= 1.1f;

					Accelerate (true);
				} else {
					inputX /= 1.01f;
					inputY /= 1.01f;
				}
			}

			if (Input.GetKey (KeyCode.S)) {
				if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) <= 1.9f) {
					Accelerate (false);
				} else if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) >= 5f) {
					inputX /= 1.1f;
					inputY /= 1.1f;
					
					Accelerate (false);
				} else {
					inputX /= 1.01f;
					inputY /= 1.01f;
				}
			}	
			if (Input.GetKey (KeyCode.A))
				inputRot += 1.5f;
			if (Input.GetKey (KeyCode.D))
				inputRot -= 1.5f;
			if (Input.GetKey (KeyCode.Space)) {
				WeaponScript weapon = GetComponent<WeaponScript> ();
				if (weapon != null) {
					weapon.Attack (false, inputRot, transform.localScale.x);
				}
			}
			movement = new Vector2 (
				speed.x * inputX,
				speed.y * inputY);
		} else {
			SyncedMovement ();
		}
			
			
		if (Mathf.Abs (inputX) < 0.005f) {
			inputX = 0f;
			movement.x = 0f;
		} else
			inputX /= 1.005f;
		if (Mathf.Abs (inputY) < 0.005f)
		{
			inputY = 0f;
			movement.y = 0f;
		}
		else 
			inputY /= 1.005f;
	}
	
	private void SyncedMovement()
	{
		syncTime += Time.deltaTime;
		rb.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	void Accelerate(bool choice) {
		var tempRot = inputRot;
		var multiplier = 1;
		//Determine if W or S pressed. If S, flip rotation by 180 degrees so our math is opposite of W. 
		//Value stored in temp variable to prevent the ship from just rotating 180 degrees like a derp.
		if (!choice) 
		{
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
		else if (tempRot == 0f || tempRot == 360f) 
		{
			inputX += (.5f * multiplier);
		}
		
		if (tempRot != 90f && tempRot != 180f && tempRot != 0f && tempRot != 360f) 
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
				inputX+= (x * 0.08f * domainX);
				inputY += (y * 0.08f * domainY);
		}
	}

	
	void FixedUpdate()
	{
		// 5 - Move the game object
		GetComponent<Rigidbody2D>().velocity = movement;
		GetComponent<Rigidbody2D> ().rotation = inputRot;
	}
}
