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

	public void Attack(bool isEnemy, float rotation, float size, int mass, int playerID, int damage, Vector3 position)
	{
		if (CanAttack)
		{
			shootCooldown = shootingRate;
			var shotTransform = Instantiate(Bullet) as Transform;

			shotTransform.position = new Vector3(position.x, position.y, 1f);
			BulletScript shot = shotTransform.gameObject.GetComponent<BulletScript>();
			shot.damage = damage;
			shot.mass = mass;
			shot.PlayerID = playerID;

			shot.transform.localScale *= (damage / 5f);
			if (shot.transform.localScale.x <= 0.01f)
				shot.transform.localScale = new Vector3(0.01f,0.01f ,01f);
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