using UnityEngine;

public class BulletScript : MonoBehaviour
{
	public int damage;
	public float mass;
	public int PlayerID;

	public bool isEnemyShot = false;
	
	void Start()
	{
		Destroy(gameObject, 7);
	}


}
