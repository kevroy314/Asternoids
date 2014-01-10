using UnityEngine;
using System.Collections;

/* 
 * Object is responsible for User Input on the main menu.
 */
public class MenuScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		//Quit on escape, load game scene on any other key
		if(Input.GetKey (KeyCode.Escape))
			Application.Quit();
		else if(Input.anyKey)
			Application.LoadLevel("Asternoids");
	}
}
