using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for inflicting gravity upon tagged objects.
 */
public class GravitySourceScript : MonoBehaviour {

	//Public variables
	public float maxGravDist = 4.0f; //Maximum distance the gravity reaches
	public float maxGravity = 35f; //Maximum amount of force the gravity can exhert
	public string[] tagsToEffect = {"Player","Enemy","Bullet"}; //Tags which when present on an object cause that object to be effected by gravity

	//Initial position of the object (used for making sure the object is collidable but never moves
	private Vector3 pos;

	//Runs on object start
	void Start() {
		//Store the initial position
		pos = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		//For every type of object we effect (by tag name)
		for(int i = 0; i < tagsToEffect.Length;i++)
		{
			//Get all the objects with that tag
			GameObject[] objs = GameObject.FindGameObjectsWithTag(tagsToEffect[i]);
			//For each object with that tag
			for(int j = 0; j < objs.Length;j++)
			{
				//Find the distance from the object to the gravity source and check if it's within range
				float distance = Vector3.Distance (objs[j].transform.position,transform.position);
				if(distance < maxGravDist)
					//Attempt to apply the gravity (this can fail if the object is destroyed before it's turn comes to be affected)
					try{
						objs[j].rigidbody2D.AddForce ((transform.position-objs[j].transform.position).normalized * (1.0f - distance / maxGravDist) * maxGravity);
					}catch(MissingComponentException){};
			}
		}
	}

	//Runs every frame
	void Update() {
		//Keep the planet where it is supposed to be
		transform.position = pos;
	}
}
