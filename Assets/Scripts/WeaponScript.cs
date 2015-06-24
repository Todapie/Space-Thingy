using UnityEngine;

public class WeaponScript : MonoBehaviour
{
	public Transform Bullet;
	public float shootingRate = 0.25f;
	private float shootCooldown;
	
	void Start()
	{
		shootCooldown = 0f;
	}
	
	void Update()
	{
		if (shootCooldown > 0)
		{
			shootCooldown -= Time.deltaTime;
		}
	}

	public void Attack(bool isEnemy, float rotation, float scaleSize, int mass, int playerID, int damage, Vector3 position)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			int numberOfIterations = 1;
			if (mass >= 250)
				numberOfIterations = 2;
			else if(mass >= 500)
				numberOfIterations = 2;
			else if(mass >= 1500)
				numberOfIterations = 1;
			else if(mass >=3500)
				numberOfIterations = 1;
			for (int i = 0; i < numberOfIterations; i++) 
			{
				var shotTransform = Instantiate(Bullet) as Transform;
				shotTransform.position = new Vector3(position.x, position.y, 1.1f);

				BulletScript shot = shotTransform.gameObject.GetComponent<BulletScript>();
				shot.damage = damage;
				shot.mass = mass;
				shot.PlayerID = playerID;
				
				shot.transform.localScale *= (scaleSize / 5f);
				if (shot != null)
				{
					shot.isEnemyShot = isEnemy;
				}
				
				MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
				if (move != null)
				{
					move.rotation = rotation;

					if (numberOfIterations == 2 && i == 0) 
					{
						if(rotation-3 < 0)
							move.rotation = rotation + 357f;
						else
							move.rotation = rotation-3f;
						//shot.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, move.rotation);
					}
					if(numberOfIterations == 2 && i == 1)
					{
						if(rotation+3 > 360)
						{
							move.rotation = rotation - 357f;
						}
						else
						{
							move.rotation = rotation+3f;
						}
						//shot.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, move.rotation);
					}
					move.size = mass;
				}
			}

		}
	}

	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
}