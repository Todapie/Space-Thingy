
using UnityEngine;
using System.Collections;

public class PlayerScript : Photon.MonoBehaviour 
{
	public GameObject bullet;
	public BulletScript bulletScript;
	public Vector2 speed = new Vector2(0, 0);
	public int size;
	private Vector2 movement;
	public float inputX = 0.0f;
	public float inputY = 0.0f;
	public float inputRot = 0.0f;
	public Rigidbody2D rb;
	public Transform Food;
	public string Name;
	private float ScaleThresholdCounter;
	private bool Accelerating = false;
	private int PlayerID = 0;
	public int Damage;
	public ParticleSystem particles;
	private WeaponScript weapon;
	private Space s;
	Vector3 m_NetworkPosition;
	Quaternion m_NetworkRotation;
	Vector2 m_Speed;
	double m_LastNetworkDataReceivedTime;
	private Vector3 m_NetworkScale;
	private double timeToReachGoal = 0.0;
	private double lastPacketTime = 0.0;
	private double lastVelocityUpdateTime = 0.0;
	private double currentPacketTime = 0.0;
	private double currentTime = 0.0;
	private Vector3 positionAtLastPacket = Vector3.zero;

	private AudioSource source;
	public AudioClip PEWPEW;

	void Start() 
	{
		s = gameObject.AddComponent<Space>();
		transform.localScale = new Vector3( 1f, 1f, 0f);
		size = 100;
		ScaleThresholdCounter = 0f;
		particles = Instantiate(particles) as ParticleSystem;
		particles.transform.position = new Vector3(transform.position.x, transform.position.y, 2f);
		particles.transform.Rotate(0, transform.rotation.z, 0);
		PhotonNetwork.sendRate = 20;
		PhotonNetwork.sendRateOnSerialize = 10;

		source = gameObject.AddComponent<AudioSource> ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.name.Contains ("Bullet")) 
		{
			BulletScript bulletScript = other.gameObject.GetComponent<BulletScript>();
			if (bulletScript.PlayerID != PlayerID) 
			{				
				Destroy (other.gameObject);
				Space s = gameObject.AddComponent<Space>();
				s.food = Food;
				if (size-bulletScript.damage > 0) 
				{
					s.DisperseFood(transform.position.x, transform.position.y, bulletScript.damage, size);
					size -= bulletScript.damage;
					transform.localScale = new Vector3( transform.localScale.x - (0.005f * bulletScript.damage), transform.localScale.y - (0.005f * bulletScript.damage), 1.1f);
				}
				else 
				{
					s.DisperseFood(transform.position.x, transform.position.y, size, size);
					Destroy (transform.gameObject);
				}
			}
		}
		if (other.name.Contains ("Food")) 
		{
			FoodScript f = other.GetComponent<FoodScript>();
			if (f.mass <= size)
			{
				size += f.mass;
				Destroy (other.gameObject);
				s.collison = false;
				s.food = Food;
				s.CreateFood();
				transform.localScale = new Vector3( transform.localScale.x + (0.01f * f.mass), transform.localScale.y + (0.01f * f.mass), 1.1f);
			}
		}
	}

	void OnCollisionEnter2D (Collision2D other) 
	{
		if (other.collider.name.Contains ("vertical")) 
		{
			if(inputX < 0)
				inputX = 4f;
			else
				inputX = -4f;
		}
		if (other.collider.name.Contains ("horizontal")) 
		{
			if (inputY < 0)
				inputY = 4f;
			else
				inputY = -4f;
		}

		if (other.collider.name.Contains ("Player")) 
		{
			Debug.Log (speed);
		}

	}

	void OnCollisionExit2D(Collision2D other)
	{
		if (other.collider.name.Contains ("Player")) 
		{
			//Debug.Log (GetComponent<Rigidbody2D>().velocity);
		}
	}

	void Shrink()
	{
		ScaleThresholdCounter += 0.000003f;
		//Should be around 1 minute for your size to decrease
		
		if (ScaleThresholdCounter >= 0.01f) 
		{
			ScaleThresholdCounter = 0f;
			if (size > 100) 
			{
				size -= 1;
			}
		}
	}

	float getCalculatedSpeed()
	{
		float calculatedSpeed = 40f;
		if (size >= 250)
			calculatedSpeed = 40f - 5f;
		else if(size >= 500)
			calculatedSpeed = 40f - 10f;
		else if(size >= 1500)
			calculatedSpeed = 40f - 15f;
		else if(size >= 3500)
			calculatedSpeed = 40f - 20f;
		return calculatedSpeed;
	}

	void WKey()
	{
		if (Input.GetKey (KeyCode.W)) 
		{
			if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) <= getCalculatedSpeed() * 0.95f) 
			{
				Accelerate (true);
			} 
			else if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) >= (getCalculatedSpeed() * 2.5f)) 
			{
				inputX /= 1.1f;
				inputY /= 1.1f;
				
				Accelerate (true);
			} 
			else 
			{
				inputX /= 1.01f;
				inputY /= 1.01f;
			}
		}
		if (Input.GetKeyUp (KeyCode.W)) 
		{
			Accelerating = false;
		}
	}

	void SKey()
	{
		if (Input.GetKey (KeyCode.S)) 
		{
			if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) <= getCalculatedSpeed() * 0.95f) 
			{
				Accelerate (false);
			} 
			else if (Mathf.Sqrt ((inputX * inputX) + (inputY * inputY)) >= (getCalculatedSpeed() * 2.5f)) 
			{
				inputX /= 1.1f;
				inputY /= 1.1f;
				Accelerate (false);
			} 
			else 
			{
				inputX /= 1.01f;
				inputY /= 1.01f;
			}
		}	
	}

	void SpaceKey()
	{
		if (Input.GetKey (KeyCode.Space)) 
		{
			int damage = Mathf.RoundToInt((float)size / 10f);
			if (weapon == null)
				weapon = GetComponent<WeaponScript> ();
			else
				weapon.Attack (false, inputRot, transform.localScale.x, size, PlayerID, damage, transform.position);
		
			source.PlayOneShot(PEWPEW, 1f);
		}
	}

	void Deaccelerate()
	{
		if (Mathf.Abs (inputX) < 0.08f && !Accelerating && inputX != 0f) 
		{
			inputX = 0f;
			movement.x = 0f;
		} 
		else
			inputX /= 1.005f;
		if (Mathf.Abs (inputY) < 0.08f && !Accelerating && inputY != 0f)
		{
			inputY = 0f;
			movement.y = 0f;
		}
		else 
			inputY /= 1.005f;
	}

	void Update() 
	{
		particles.transform.position = new Vector3(transform.position.x, transform.position.y, 2f);
		particles.transform.Rotate(0, transform.rotation.z, 0);

		Damage = Mathf.RoundToInt((float)size / 10f);

		Shrink ();
		if (inputRot < 0f)
			inputRot += 360f;
		if (inputRot > 360f)
			inputRot -= 360f;
		if (photonView.isMine) 
		{
			WKey();
			SKey();
			SpaceKey();

			if (Input.GetKey (KeyCode.A))
				inputRot += 2f;
			if (Input.GetKey (KeyCode.D))
				inputRot -= 2f;

			movement = new Vector2 (speed.x * inputX, speed.y * inputY);
			GetComponent<Rigidbody2D>().velocity = movement;
			GetComponent<Rigidbody2D> ().rotation = inputRot;
		}
		else
		{
			//double timeToReachGoal = currentPacketTime - lastPacketTime;
			//currentTime += Time.deltaTime;
			//transform.position = Vector3.Lerp(positionAtLastPacket, m_NetworkPosition, (float)(currentTime / timeToReachGoal));
			UpdateNetworkedPosition();
			UpdateNetworkedRotation();
			UpdateNetworkScale();
			UpdateNetworkVelocity();
		}
		Deaccelerate();
	}

	void Accelerate(bool choice) 
	{
		Accelerating = true;
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
		if (inputRot == 270f)
			inputY -= (.08f * multiplier);
		else if(inputRot == 90f)
			inputY += (1f * multiplier);
		else if (tempRot == 180f)
			inputX -= (1f * multiplier);
		else if(tempRot == 180f && multiplier == -1)
			inputX += (1f * multiplier);
		else if (tempRot == 360f) 
			inputX += (1f * multiplier);
		else if (tempRot == 0f && inputRot == 180f)
			inputX -= (1f * multiplier);
		else if (tempRot == 0f && inputRot == 0f)
			inputX += (1f * multiplier);

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
			} 
			else if (tempRot > 180f && tempRot < 270f) 
			{
				domainX = -1;
				domainY = -1;
			} 
			else if (tempRot > 270f && tempRot < 360f) 
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
				inputX+= (x * 1f * domainX);
				inputY += (y * 1f * domainY);
		}
	}

	void OnPhotonSerializeView( PhotonStream stream, PhotonMessageInfo info )
	{
		//Multiple components need to synchronize values over the network.
		//The SerializeState methods are made up, but they're useful to keep
		//all the data separated into their respective components
		
		SerializeState( stream, info );
		
		//ShipVisuals.SerializeState( stream, info );
		//ShipMovement.SerializeState( stream, info );
	}

	public void SerializeState( PhotonStream stream, PhotonMessageInfo info )
	{
		//We only need to synchronize a couple of variables to be able to recreate a good
		//approximation of the ships position on each client
		//There is a lot of smoke and mirrors happening here
		//Check out Part 1 Lesson 2 http://youtu.be/7hWuxxm6wsA for more detailed explanations
		if( stream.isWriting == true )
		{
			stream.SendNext( transform.position );
			stream.SendNext( transform.rotation );
			stream.SendNext( transform.GetComponent<Rigidbody2D>().velocity );
			stream.SendNext(transform.localScale);
		}
		else
		{
			currentTime = 0.0;
			positionAtLastPacket = transform.position;
			m_NetworkPosition = (Vector3)stream.ReceiveNext();
			m_NetworkRotation = (Quaternion)stream.ReceiveNext();
			m_Speed = (Vector2)stream.ReceiveNext();
			m_NetworkScale = (Vector3)stream.ReceiveNext();

			lastPacketTime = m_LastNetworkDataReceivedTime;
			m_LastNetworkDataReceivedTime = info.timestamp;
		}
	}

	void UpdateNetworkVelocity()
	{
	//	if(lastPacketTime > lastVelocityUpdateTime)
	//	{
			speed = m_Speed;
			GetComponent<Rigidbody2D> ().velocity = m_Speed;
			lastVelocityUpdateTime = lastPacketTime;
	//	}
	}

	void UpdateNetworkScale()
	{
		transform.localScale = Vector3.MoveTowards(transform.localScale, m_NetworkScale, 180f * Time.deltaTime);
	}

	void UpdateNetworkedPosition()
	{
		//Here we try to predict where the player actually is depending on the data we received through the network
		//Check out Part 1 Lesson 2 http://youtu.be/7hWuxxm6wsA for more detailed explanations
		float pingInSeconds = (float)PhotonNetwork.GetPing() * 0.001f;
		float timeSinceLastUpdate = (float)( PhotonNetwork.time - m_LastNetworkDataReceivedTime );
		float totalTimePassed = pingInSeconds + timeSinceLastUpdate;
		
		Vector3 exterpolatedTargetPosition = m_NetworkPosition
			+ transform.forward * m_Speed.sqrMagnitude * totalTimePassed;
		
		
		Vector3 newPosition = Vector3.MoveTowards( transform.position
		                                          , exterpolatedTargetPosition
		                                          , m_Speed.sqrMagnitude * Time.deltaTime );
		
		if( Vector2.Distance( transform.position, exterpolatedTargetPosition ) > 2f )
		{
			newPosition = exterpolatedTargetPosition;
		}
		
		//newPosition.y = Mathf.Clamp( newPosition.y, 0.5f, 50f );
		
		transform.position = newPosition;
	}
	
	void UpdateNetworkedRotation()
	{
		transform.rotation = Quaternion.RotateTowards(
			transform.rotation,
			m_NetworkRotation, 180f * Time.deltaTime
			);
	}

}
