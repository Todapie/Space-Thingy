using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public int damage;

	public bool isEnemyShot = false;
	
	void Start()
	{
		// 2 - Limited time to live to avoid any leak
		Destroy(gameObject, 20); // 20sec
	}


}
