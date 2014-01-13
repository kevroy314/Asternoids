using UnityEngine;
using System.Collections;

public class LevelManagerScript : MonoBehaviour {
	public enum GameState {Running = 0, Win = 1, Lose = 2};

	private EnemyManagerScript[] enemySpawnerScripts;
	private CharacterControllerScript playerScript;
	public GameState gameState;
	public float totalEnemiesLeft;
	public GameObject player;

	// Use this for initialization
	void Start () {
		//Populate the list of gravity objects to watch for level state
		enemySpawnerScripts = gameObject.GetComponentsInChildren <EnemyManagerScript>();
		playerScript = player.GetComponent<CharacterControllerScript> ();
		gameState = GameState.Running;
	}
	
	// Update is called once per frame
	void Update () {
		totalEnemiesLeft = 0f;
		for(int i = 0; i < enemySpawnerScripts.Length;i++)
			totalEnemiesLeft += enemySpawnerScripts[i].enemyCount + enemySpawnerScripts[i].totalEnemyCount;
		if(gameState == GameState.Running)
		{
			if(totalEnemiesLeft <= 0)
				gameState = GameState.Win;
			else if(playerScript.hp <= 0) 
				gameState = GameState.Lose;
		}

	}

	//Update the game state UI
	void OnGUI () {
		if(gameState == GameState.Lose)
		{
			GUI.color = Color.red;
			GUI.Label (new Rect(Screen.width/2,Screen.height/2,400,100),"You Lose!");
		}
		else if(gameState == GameState.Win)
		{
			GUI.color = Color.green;
			GUI.Label (new Rect(Screen.width/2,Screen.height/2,400,100),"You Win!");
		}
	}
}
