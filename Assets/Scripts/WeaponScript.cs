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

	public void Attack(bool isEnemy, float rotation)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;

			var shotTransform = Instantiate(Bullet) as Transform;

			shotTransform.position = transform.position;

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

	public bool CanAttack
	{
		get
		{
			return shootCooldown <= 0f;
		}
	}
}