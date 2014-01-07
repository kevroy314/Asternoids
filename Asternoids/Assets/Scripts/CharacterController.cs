using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {
	//Keyboard interfacing properties
	public KeyCode forwardKey = KeyCode.W; //Key for moving forward
	public KeyCode reverseKey = KeyCode.S; //Key for moving in reverse
	public KeyCode leftKey = KeyCode.A; //Key for moving left (strafe)
	public KeyCode rightKey = KeyCode.D; //Key for moving right (strafe)
	public KeyCode fireKey = KeyCode.Space; //Key for shooting

	//Bullet properties
	public GameObject bulletPrefab; //Prefabricated 2D rigid body for bullet
	public float bulletSpeed = 50f; //Speed the bullets travel

	//Ship performance properties
	public float acceleration = 5f; //Rate of acceleration
	public float maxSpeed = 10f; //Maximum movement speed

	//External objects
	public Camera dependentCamera; //Camera for world transform

	//Player properties
	public float damage = 0.0f;

	//Update is called once per frame
	void Update () {
		//Get current mouse position and transform rleative to object and camera
		Vector3 mousePos = Input.mousePosition;

		//Transform object position using camera world view
		Vector3 objectPos = dependentCamera.WorldToScreenPoint (transform.position);
		//Transform mouse position relative to object and camera
		mousePos.x = mousePos.x - objectPos.x;
		mousePos.y = mousePos.y - objectPos.y;
		mousePos.z = dependentCamera.transform.position.z - transform.position.z;
		//Compute rotation angle and set the object to be rotated
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg - 90f));

		//Process movement
		if(Input.GetKey (forwardKey))
			rigidbody2D.AddForce (new Vector2(0,acceleration));
			//rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z+90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z+90)*Mathf.Deg2Rad))*acceleration);
		else if(Input.GetKey (reverseKey))
			rigidbody2D.AddForce (new Vector2(0,-acceleration));
			//rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z-90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z-90)*Mathf.Deg2Rad))*acceleration);
		if(Input.GetKey (leftKey))
			rigidbody2D.AddForce (new Vector2(-acceleration,0));
			//rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z-180)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z-180)*Mathf.Deg2Rad))*acceleration);
		else if(Input.GetKey (rightKey))
			rigidbody2D.AddForce (new Vector2(acceleration,0));
			//rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z)*Mathf.Deg2Rad))*acceleration);

		//Set maximum velocity magnitude
		if(rigidbody2D.velocity.magnitude>maxSpeed)
			rigidbody2D.velocity *= (1 - (rigidbody2D.velocity.magnitude-maxSpeed)/rigidbody2D.velocity.magnitude);

		//Fire bullets
		if(Input.GetKey (fireKey)||Input.GetMouseButton(0))
		{
			audio.Play ();
			Vector2 direction = new Vector2(Mathf.Cos ((transform.eulerAngles.z+90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z+90)*Mathf.Deg2Rad));
			GameObject bullet = Instantiate(bulletPrefab, transform.position+new Vector3(direction.x,direction.y,0f), transform.rotation) as GameObject;
			//bullet.transform.parent = transform;
			bullet.rigidbody2D.velocity = direction * bulletSpeed;
		}

		if(Input.GetKey (KeyCode.Escape))
			Application.LoadLevel ("Main Menu");
	}

	void OnCollisionEnter2D(Collision2D collision) {
		damage++;
	}
}
