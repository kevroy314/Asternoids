using UnityEngine;
using System.Collections;

public class EnemyAI : MonoBehaviour {
	//Enemy performance
	public float acceleration = 3f; //How quickly the enemy can acceleration/change directions
	public float maxSpeed = 5f; //Maximum enemy speed

	//AI properties
	public GameObject target; //Target the enemy ship chases

	private Animator anim;

	//Dependent objects
	public GameObject enemyManager; //For reporting killed ships

	public float hp = 100f;

	void Start() {
		anim = GetComponent<Animator>();
	}

	// Update is called once per frame
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
		if(anim.GetCurrentAnimatorStateInfo (0).IsTag ("DestroyObject"))
		{
			EnemyManagerScript enemyCom = enemyManager.GetComponent<EnemyManagerScript>();
			enemyCom.reportDeath();
			DestroyObject (gameObject);
		}
		if(anim.GetCurrentAnimatorStateInfo (0).IsTag ("ResetShipHit"))
		{
			anim.SetBool("ShipHit",false);
		}
	}

	void OnCollisionEnter2D(Collision2D collision) {
		float damage = float.MaxValue;
		BulletManagerScript bullet = collision.gameObject.GetComponent<BulletManagerScript>();
		if(bullet!=null)
			damage = bullet.damage;
		if(collision.gameObject.tag != "Planet")
		{
			anim.SetBool("ShipHit",true);
			hp-=damage;
			if(hp<=0)
			{
				anim.SetBool("ShipHasDied",true);
				tag = "Untagged";
				rigidbody2D.isKinematic = true;
				audio.Play ();
			}
		}
	}
}
