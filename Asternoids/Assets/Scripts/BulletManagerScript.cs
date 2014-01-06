using UnityEngine;
using System.Collections;

public class BulletManagerScript : MonoBehaviour {
	//Distance from origin for memory cleanup
	public float destroyDistance = 200f; //Distance from origin at which point a bullet disappears

	//Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(rigidbody2D.velocity.y, rigidbody2D.velocity.x) * Mathf.Rad2Deg - 90f));
		//Check boundry conditions and destroy if exceeded
		if(transform.position.magnitude > destroyDistance)
			DestroyObject (gameObject);
	}

	void OnCollisionEnter2D(Collision2D collision) {
		DestroyObject (gameObject);
	}
}