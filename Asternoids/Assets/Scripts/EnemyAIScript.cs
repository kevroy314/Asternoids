using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for the enemy AI.
 */
public class EnemyAIScript : MonoBehaviour {

	//Public variables

	//Enemy performance
	public float acceleration = 5f; //How quickly the enemy can acceleration/change directions
	public float maxSpeed = 7f; //Maximum enemy speed
	public float hp = 50f; //Enemy health

	//Bullet properties
	public GameObject bulletPrefab; //Prefabricated 2D rigid body for bullet
	public float bulletSpeed = 20f; //Speed the bullets travel
	public float accuracy = 0.95f; //Accuracy is the percentage potential random offset from the intended direction of fire
	public float maxAccuracyOffset = 10f; //The maximum amount off of center that the bullet can fire
	public float weaponKick = 0.2f; //This force is applied every time the character fires.
	public float fireProbability = 0.3f;

	//Dependent objects
	public GameObject target; //Target the enemy ship chases
	public GameObject enemyManager; //For reporting killed ships

	//Private variables
	private Animator anim; //Animator for monitoring animation state

	//Private Bullet properties
	private float accuracyFactor; //Accuracy factor is precomputed for efficiency
	private float accuracyOffset; //Accuracy offset is persistent between shots and determines the direction the weapon is pointed

	private bool hitPlanet = false;
	private Vector2 hitPlanetPosition;
	private int reverseCounter = 0;
	private int reverseCount = 10;
	//Runs when object starts
	void Start() {
		//Precompute accuracy information based on input variables
		accuracyFactor = 360*(1-accuracy);
		accuracyOffset = 0f;
		//Get animator component for monitoring animation state
		anim = GetComponent<Animator>();
	}

	//Update is called once per frame
	void Update () {
		//Calculate a direction for the target
		Vector2 direction;
		if(hitPlanet)
		{
			reverseCounter++;
			if(reverseCounter> reverseCount)
			{
				hitPlanet = false;
				reverseCounter = 0;
			}
			direction = ((new Vector2(transform.position.x,transform.position.y))-hitPlanetPosition).normalized;
		}
		else
		{
			Vector3 direction3 = (target.transform.position-transform.position).normalized;
			direction = new Vector2(direction3.x,direction3.y);
		}

		//Rotate the ship to face the direction of the target
		transform.rotation = Quaternion.Euler (new Vector3(0f,0f,Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f));

		//Go towards the target
		rigidbody2D.AddForce (new Vector2(direction.x,direction.y)*acceleration);

		//Limit velocity to maximum
		if(rigidbody2D.velocity.magnitude>maxSpeed)
			rigidbody2D.velocity *= (1 - (rigidbody2D.velocity.magnitude-maxSpeed)/rigidbody2D.velocity.magnitude);

		//Check if the enemy should be destroyed (based on it's animation state)
		if(anim.GetCurrentAnimatorStateInfo (0).IsTag ("DestroyObject"))
		{
			//Report the death to the manager
			EnemyManagerScript enemyCom = enemyManager.GetComponent<EnemyManagerScript>();
			enemyCom.reportDeath();

			//Destroy the object
			DestroyObject (gameObject);


		}

		//Fire bullets
		if(Random.value < fireProbability)
		{
			//Determine the angle of the bullet
			float bulletAngle = (transform.eulerAngles.z+90);
			
			//Change the accuracy offset to determine the new direction of fire and bound it appropriately
			accuracyOffset += (Random.value-0.5f)*accuracyFactor;
			if(accuracyOffset>maxAccuracyOffset) accuracyOffset = maxAccuracyOffset;
			if(accuracyOffset<-maxAccuracyOffset) accuracyOffset = -maxAccuracyOffset;
			
			//Generate the actual angle of fire
			bulletAngle += accuracyOffset;
			direction = new Vector2(Mathf.Cos (bulletAngle*Mathf.Deg2Rad), Mathf.Sin (bulletAngle*Mathf.Deg2Rad));
			
			//Create a bullet and set it's speed
			GameObject bullet = Instantiate(bulletPrefab, transform.position+new Vector3(direction.x,direction.y,0f)*transform.localScale.x, transform.rotation) as GameObject;
			bullet.rigidbody2D.velocity = direction * bulletSpeed;
			
			//Apply weapon kick to player
			rigidbody2D.AddForce (new Vector2(-direction.x*weaponKick,-direction.y*weaponKick));
		}

		//Check if the enemy hit animation is complete and reset it
		if(anim.GetCurrentAnimatorStateInfo (0).IsTag ("ResetShipHit"))
			anim.SetBool("ShipHit",false);
	}

	//Triggers at the start of any collisions
	void OnCollisionEnter2D(Collision2D collision) {
		//By default, all collisions cause maximum damage
		float damage = float.MaxValue;

		//Get a bullet manager script if the collided object has one
		BulletManagerScript bullet = collision.gameObject.GetComponent<BulletManagerScript>();
		//If it has one, use it's damage instead of the default damage
		if(bullet!=null)
			damage = bullet.damage;

		string objectTag = collision.gameObject.tag;

		//If the object is one with which we should apply damage on collision
		if(objectTag != "Planet" && objectTag != "EnemyBullet")
		{
			//Trigger the hit animation
			anim.SetBool("ShipHit",true);
			//Apply damage
			hp-=damage;

			//Check for death condition
			if(hp<=0)
			{
				//Trigger death animation
				anim.SetBool("ShipHasDied",true);
				//Stop being effected by gravity
				rigidbody2D.isKinematic = true;
				//Play death sound effect
				audio.Play ();
			}
		}
		else if (objectTag == "Planet")
		{
			hitPlanet = true;
			hitPlanetPosition = collision.gameObject.transform.position;
		}
	}
}
