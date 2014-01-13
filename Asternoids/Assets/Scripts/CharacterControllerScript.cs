using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for Player ship control, movement, and properties. 
 */
public class CharacterControllerScript : MonoBehaviour {

	//Keyboard interfacing properties
	public KeyCode forwardKey = KeyCode.W; //Key for moving forward
	public KeyCode reverseKey = KeyCode.S; //Key for moving in reverse
	public KeyCode leftKey = KeyCode.A; //Key for moving left (strafe)
	public KeyCode rightKey = KeyCode.D; //Key for moving right (strafe)
	public KeyCode fireKey = KeyCode.Space; //Key for shooting
	public bool playerRelativeControls = false; //If true, the ship travels in the direction it faces, else it travels relative to the camera

	//Bullet properties
	public GameObject bulletPrefab; //Prefabricated 2D rigid body for bullet
	public float bulletSpeed = 20f; //Speed the bullets travel
	public float accuracy = 0.95f; //Accuracy is the percentage potential random offset from the intended direction of fire
	public float maxAccuracyOffset = 10f; //The maximum amount off of center that the bullet can fire
	public float weaponKick = 0.2f; //This force is applied every time the character fires.

	//Ship properties
	public float acceleration = 5f; //Rate of acceleration
	public float maxSpeed = 10f; //Maximum movement speed
	public float hp = 1000.0f; //Amount of damage accumulated

	//External objects
	public Camera dependentCamera; //Camera for world transform

	//Private variables
	private Animator anim;
	private FollowPlayerScript cameraScript; //Script for asking the camera to shake

	//Private Bullet properties
	private float accuracyFactor; //Accuracy factor is precomputed for efficiency
	private float accuracyOffset; //Accuracy offset is persistent between shots and determines the direction the weapon is pointed

	//Run when object is created
	void Start() {
		//Precompute accuracy information based on input variables
		accuracyFactor = 360*(1-accuracy);
		accuracyOffset = 0f;
		//Get animator for animation state monitoring
		anim = gameObject.GetComponent <Animator>();
		cameraScript = dependentCamera.GetComponent<FollowPlayerScript> ();
	}

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

		//Check for movement
		bool movement = false;

		//Process movement
		if(Input.GetKey (forwardKey)) { //Move forward
			if(playerRelativeControls)
				rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z+90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z+90)*Mathf.Deg2Rad))*acceleration);
			else
				rigidbody2D.AddForce (new Vector2(0,acceleration));
			movement = true;
		}
		else if(Input.GetKey (reverseKey)) { //Move backwards
			if(playerRelativeControls)
				rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z-90)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z-90)*Mathf.Deg2Rad))*acceleration);
			else
				rigidbody2D.AddForce (new Vector2(0,-acceleration));
			movement = true;
		}
		if(Input.GetKey (leftKey)) { //Strafe Left
			if(playerRelativeControls)
				rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z-180)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z-180)*Mathf.Deg2Rad))*acceleration);
			else
				rigidbody2D.AddForce (new Vector2(-acceleration,0));
			movement = true;
		}
		else if(Input.GetKey (rightKey)) { //Strafe Right
			if(playerRelativeControls)
				rigidbody2D.AddForce (new Vector2(Mathf.Cos ((transform.eulerAngles.z)*Mathf.Deg2Rad), Mathf.Sin ((transform.eulerAngles.z)*Mathf.Deg2Rad))*acceleration);
			else
				rigidbody2D.AddForce (new Vector2(acceleration,0));
			movement = true;
		}

		//Trigger the acceleration animation if the user is providing input to request acceleration
		anim.SetBool ("ShipAccelerating",movement);

		//Set maximum velocity magnitude
		if(rigidbody2D.velocity.magnitude>maxSpeed)
			rigidbody2D.velocity *= (1 - (rigidbody2D.velocity.magnitude-maxSpeed)/rigidbody2D.velocity.magnitude);

		//Fire bullets
		if(Input.GetKey (fireKey)||Input.GetMouseButton(0))
		{
			//Determine the angle of the bullet
			float bulletAngle = (transform.eulerAngles.z+90);

			//Change the accuracy offset to determine the new direction of fire and bound it appropriately
			accuracyOffset += (Random.value-0.5f)*accuracyFactor;
			if(accuracyOffset>maxAccuracyOffset) accuracyOffset = maxAccuracyOffset;
			if(accuracyOffset<-maxAccuracyOffset) accuracyOffset = -maxAccuracyOffset;

			//Generate the actual angle of fire
			bulletAngle += accuracyOffset;
			Vector2 direction = new Vector2(Mathf.Cos (bulletAngle*Mathf.Deg2Rad), Mathf.Sin (bulletAngle*Mathf.Deg2Rad));

			//Create a bullet and set it's speed
			GameObject bullet = Instantiate(bulletPrefab, transform.position+new Vector3(direction.x,direction.y,0f)*transform.localScale.x, transform.rotation) as GameObject;
			bullet.rigidbody2D.velocity = direction * bulletSpeed;

			//Apply weapon kick to player
			rigidbody2D.AddForce (new Vector2(-direction.x*weaponKick,-direction.y*weaponKick));
		}

		//On escape, go to main menu
		if(Input.GetKey (KeyCode.Escape))
			Application.LoadLevel ("Main Menu");
	}

	//Triggers at the start of any collisions
	void OnCollisionEnter2D(Collision2D collision) {
		//Add damage on any collision
		hp--;
		cameraScript.ShakeCamera (0.5f);
	}
}
