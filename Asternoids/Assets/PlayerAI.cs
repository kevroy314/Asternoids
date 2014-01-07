using UnityEngine;
using System.Collections;

public class PlayerAI : MonoBehaviour {
	
	public float fireProbability;
	public GameObject bulletPrefab;
	public float bulletSpeed;
	public float playerSpeed;
	private float x = 0;
	// Update is called once per frame
	void Update () {
		x+=0.05f;
		rigidbody2D.AddForce ((new Vector2(-transform.position.x,-transform.position.y)).normalized);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(Mathf.Cos (x), Mathf.Sin (x)) * Mathf.Rad2Deg - 90f));

		//Fire bullets
		if(Random.value < fireProbability)
		{
			Vector2 direction = new Vector2(Mathf.Cos ((transform.eulerAngles.z+90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z+90)*Mathf.Deg2Rad));
			GameObject bullet = Instantiate(bulletPrefab, transform.position+new Vector3(direction.x,direction.y,0f), transform.rotation) as GameObject;
			bullet.rigidbody2D.velocity = direction * bulletSpeed;
		}
	}
}