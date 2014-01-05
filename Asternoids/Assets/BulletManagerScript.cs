using UnityEngine;
using System.Collections;

public class BulletManagerScript : MonoBehaviour {
	//Distance from origin for memory cleanup
	public float destroyDistance = 200f; //Distance from origin at which point a bullet disappears

	//Update is called once per frame
	void Update () {
		//Check boundry conditions and destroy if exceeded
		if(transform.position.magnitude > destroyDistance)
			DestroyObject (gameObject);
	}
}
