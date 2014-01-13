using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for automated player behavior.
 */
public class PlayerAIScript: MonoBehaviour {

	//Public variables

	//Behavior parameters
	public float fireProbability = 0.8f; //Abstractly represents the fire rate (0<=x<=1, higher means fire faster)
	public float playerSpeed = 5f; //Speed the player will move (any number, higher makes faster movement)
	public float spinRate = 0.05f; //How quickly the player will spin (0<=x<=2*pi, lower makes slower spin)
	//Bullet parameters
	public GameObject bulletPrefab; //Bullet the player will shoot
	public float bulletSpeed = 25f; //Speed of the bullets (any number, higher makes faster movement)

	//Private variables
	private float x = 0f; //Used for making the player spin

	// Update is called once per frame
	void Update () {
		//Update the spin variable
		x+=spinRate;

		//Move player in direction it is pointing
		rigidbody2D.AddForce ((new Vector2(-transform.position.x,-transform.position.y)).normalized);
		//Rotate player based on new x
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(Mathf.Cos (x), Mathf.Sin (x)) * Mathf.Rad2Deg - 90f));

		//Fire bullets based on fireProbability
		if(Random.value < fireProbability)
		{
			//Fire in direction ship is pointing
			Vector2 direction = new Vector2(Mathf.Cos ((transform.eulerAngles.z+90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z+90)*Mathf.Deg2Rad));
			//Make new bullet
			GameObject bullet = Instantiate(bulletPrefab, transform.position+new Vector3(direction.x,direction.y,0f), transform.rotation) as GameObject;
			//Set bullet speed
			bullet.rigidbody2D.velocity = direction * bulletSpeed;
		}
	}
}