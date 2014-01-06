using UnityEngine;
using System.Collections;

public class EnemyManagerScript : MonoBehaviour {
	//Spawn location properties
	public float innerSpawnRadius = 10f; //Distance from center where spawns start
	public float outerSpawnRadius = 100f; //Distance from center when spawns stop
	public Vector2 spawnCenter = new Vector2(0f,0f); //Center of spawn region

	//External object dependencies
	public GameObject enemyPrefab; //Enemy prefab
	public GameObject target; //Target for enemies to chase

	//Spawn frequency management
	public float spawnProbability = 0.01f; //Probability of spawning on any iteration
	public float maxEnemies = 25f; //Maximum number of enemies that can exist at once
	public float enemyCount = 0; //Current number of enemies

	//Random generator for enemy locations - creates a donut shape over many iterations
	public static Vector2 GetRandomInDonut(float min, float max)
	{
		float rot = Random.Range(0f, Mathf.PI*2);
		Ray ray = new Ray(Vector3.zero, new Vector3(Mathf.Cos (rot), Mathf.Sin (rot),0f));
		Vector3 point3 = ray.GetPoint( Random.Range(min, max) );
		return new Vector2(point3.x,point3.y);
	}

	//Update is called once per frame
	void Update () {
		//If we haven't hit the max enemy count and we randomly decide to spawn an enemy
		if(Random.value < spawnProbability && enemyCount < maxEnemies)
		{
			//Generate enemy with random spawn point and orientation
			GameObject enemy = Instantiate(enemyPrefab, GetRandomInDonut(innerSpawnRadius,outerSpawnRadius)+spawnCenter, Quaternion.Euler(new Vector3(0, 0, Random.Range (0f,360f)))) as GameObject;
			//Set the enemy target
			EnemyAI enemyCom = enemy.GetComponent<EnemyAI>();
			enemyCom.target = target;
			enemyCom.enemyManager = gameObject;
			enemyCount++;
		}
	}

	public void reportDeath()
	{
		enemyCount--;
	}
}
