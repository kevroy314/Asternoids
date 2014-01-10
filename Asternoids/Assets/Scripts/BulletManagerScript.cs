using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for managing a bullet, it's collisions, animations, and lifecycle.
 */
public class BulletManagerScript : MonoBehaviour {

	//Public variables
	public float destroyDistance = 200f; //Distance from origin at which point a bullet disappears
	public float damage = 0.1f; //Amount of damage the bullet does when it hits something

	//Private variables
	private Animator anim; //Animator (for observing animation state)

	//When object starts, this runs
	void Start() {
		//Play sound effect
		audio.Play ();

		//Get animator for animation state monitoring
		anim = GetComponent<Animator>();
	}

	//Update is called once per frame
	void Update () {
		//Make the bullet face the direction it's going
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg - 90f));

		//Check boundry conditions and animation state and destroy if the bullet is too far away or has been destroyed be it's animation state
		if(transform.position.magnitude > destroyDistance || anim.GetCurrentAnimatorStateInfo (0).IsTag ("DestroyObject"))
			DestroyObject (gameObject);
	}

	//Triggers at the start of any collisions
	void OnCollisionEnter2D(Collision2D collision) {
		//Initiate the bullet hit animation (starts bullet death)
		anim.SetBool("BulletHit",true);
	}
}