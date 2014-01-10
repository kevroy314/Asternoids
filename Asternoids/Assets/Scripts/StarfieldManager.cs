using UnityEngine;
using System.Collections;

public class StarfieldManager : MonoBehaviour {
	public GameObject[] starPrefabs;
	
	// Use this for initialization
	void Start () {
		for(int i = 0; i < 5000; i++)
		{
			//Generate enemy with random spawn point and orientation
			GameObject star = Instantiate(starPrefabs[Random.Range (0,starPrefabs.Length)], Random.insideUnitSphere*200, Quaternion.Euler(new Vector3(0, 0, 0))) as GameObject;
			star.transform.parent = gameObject.transform;
			float scale = Random.value;
			star.transform.localScale = new Vector3(scale,scale,0f);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
