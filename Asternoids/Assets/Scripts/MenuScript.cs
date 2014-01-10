using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {

	void Start () {

	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKey (KeyCode.Escape))
			Application.Quit();
		else if(Input.anyKey)
			Application.LoadLevel("Asternoids");
	}
}
