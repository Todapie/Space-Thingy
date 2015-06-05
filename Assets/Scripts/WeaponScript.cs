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

	public void Attack(bool isEnemy, float rotation, float size)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			var shotTransform = Instantiate(Bullet) as Transform;

			shotTransform.position = BulletPositioning(rotation, size);

			BulletScript shot = shotTransform.gameObject.GetComponent<BulletScript>();
			if (shot != null)
			{
				shot.isEnemyShot = isEnemy;
			}

			MoveScript move = shotTransform.gameObject.GetComponent<MoveScript>();
			if (move != null)
			{
				move.rotation = rotation;
			}
		}
	}

	public Vector3 BulletPositioning(float rotation, float size) 
	{
		var x = 0f;
		var y = 0f;
		var domainX = 1;
		var domainY = 1;

		if (rotation >= 90f && rotation <= 180f) 
		{
			domainX = -1;
		} else if (rotation > 180f && rotation < 270f) 
		{
			domainX = -1;
			domainY = -1;
		} 
		else if (rotation >= 270f && rotation < 360f) 
		{
			domainY = -1;
		}
		
		float sizeMultiplier = size / 0.05f;
		var ratio = Mathf.Abs(Mathf.Tan(rotation * Mathf.PI / 180f));
		
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
		
		x = x * Mathf.Sqrt (0.16f) * domainX;
		y = y * Mathf.Sqrt (0.16f) * domainY;
		
		float sumX = transform.position.x + (x * sizeMultiplier);
		float sumY = transform.position.y + (y * sizeMultiplier);

		return new Vector3(sumX, sumY, transform.position.z);
	}

	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
}