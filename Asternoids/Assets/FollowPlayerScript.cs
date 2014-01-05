using UnityEngine;
using System.Collections;

public class FollowPlayerScript : MonoBehaviour {
	//Camera target
	public Transform target; //Player for camera to follow
	
	//Camera movement properties
	public float height = 3.0f; //Distance from camera center to object
	public float damping = 1000.0f; //Smoothing effect for motion (higher allows for smoother camera following, lower causes jerking)

	//Zoom properties
	public float cameraMin = 5f; //Minimum zoom distance
	public float cameraMax = 50f; //Maximum zoom distance
	public float cameraZoomGrowthRate = 0.1f; //Rate at which zoom increases (closer to 0 means slower zoom, farther from 0 means faster zoom)

	//Update is called once per frame
	void Update () {
		//Camera follow
		transform.position = Vector3.Lerp (transform.position, target.TransformPoint(0, height, -1f), Time.deltaTime * damping);

		//Camera zoom
		if( Input.GetAxis("Mouse ScrollWheel") > 0 && camera.orthographicSize > cameraMin)
		{
			camera.orthographicSize -= camera.orthographicSize*cameraZoomGrowthRate;
			camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, cameraMin, cameraMax);
		}
		else if(Input.GetAxis("Mouse ScrollWheel") < 0 && camera.orthographicSize < cameraMax)
		{
			camera.orthographicSize += camera.orthographicSize*cameraZoomGrowthRate;
			camera.orthographicSize = Mathf.Clamp(camera.orthographicSize, cameraMin, cameraMax);
		}
	}
}