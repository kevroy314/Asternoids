using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for automated player behavior.
 */
public class StarfieldManager : MonoBehaviour {

	//Prefabs used to generate stars (can be any number of star objects)
	public GameObject[] starPrefabs;
	public int numberOfStars = 5000;

	// Use this for initialization
	void Start () {
		for(int i = 0; i < numberOfStars; i++)
		{
			//Generate star with random spawn point and orientation
			GameObject star = Instantiate(starPrefabs[Random.Range (0,starPrefabs.Length)], Random.insideUnitSphere*200, Quaternion.Euler(Vector3.zero)) as GameObject;
			//Set the parent of the star to be the Starfield Manager
			star.transform.parent = gameObject.transform;
			//Scale the star based on a random value (but make that the same in both x and y axis)
			float scale = Random.value;
			star.transform.localScale = new Vector3(scale,scale,0f);
		}
	}
}
