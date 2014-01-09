using UnityEngine;
using System.Collections;

public class GravitySourceScript : MonoBehaviour {
	public float maxGravDist = 4.0f;
	public float maxGravity = 35f;
	public string[] tagsToEffect = {"Player","Enemy","Bullet"};

	private Vector3 pos;

	void Start() {
		pos = transform.position;
	}

	// Update is called once per frame
	void FixedUpdate () {
		for(int i = 0; i < tagsToEffect.Length;i++)
		{
			GameObject[] objs = GameObject.FindGameObjectsWithTag(tagsToEffect[i]);
			for(int j = 0; j < objs.Length;j++)
			{
				float distance = Vector3.Distance (objs[j].transform.position,transform.position);
				if(distance < maxGravDist)
				try{
					objs[j].rigidbody2D.AddForce ((transform.position-objs[j].transform.position).normalized * (1.0f - distance / maxGravDist) * maxGravity);
				}catch(MissingComponentException){};
			}
		}
	}

	void Update() {
		transform.position = pos;
	}
}
