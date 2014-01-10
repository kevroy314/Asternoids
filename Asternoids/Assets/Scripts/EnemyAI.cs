using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for the enemy AI.
 */
public class EnemyAI : MonoBehaviour {

	//Public variables

	//Enemy performance
	public float acceleration = 5f; //How quickly the enemy can acceleration/change directions
	public float maxSpeed = 7f; //Maximum enemy speed
	public float hp = 50f; //Enemy health

	//Dependent objects
	public GameObject target; //Target the enemy ship chases
	public GameObject enemyManager; //For reporting killed ships

	//Private variables
	private Animator anim; //Animator for monitoring animation state

	//Runs when object starts
	void Start() {
		//Get animator component for monitoring animation state
		anim = GetComponent<Animator>();
	}

	//Update is called once per frame
	void Update () {
		//Calculate a direction for the target
		Vector3 direction3 = (target.transform.position-transform.position).normalized;
		Vector2 direction = new Vector2(direction3.x,direction3.y);

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

		//If the object is one with which we should apply damage on collision
		if(collision.gameObject.tag != "Planet")
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
	}
}
