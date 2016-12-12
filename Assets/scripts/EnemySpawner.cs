using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	public GameObject[] enemies;

	public float spawnTime = 2.0f;
	public float decreaseSpawnTime = 0.01f; // Each spawnTime
	public float startSpawnTime;
	public float minSpawnTime = 1.0f;
	float currentSpawnTime = 0.0f;

	public GameObject waitUntil;
	bool waited = false;

	public bool withHelmet = false;
	public bool withHoverboard = false;
	public int health = 10;

	public int minDropGold = 1;
	public int maxDropGold = 1;

	public GameObject gameController;
	GameController controller;

	// Use this for initialization
	void Start () {
		controller = gameController.GetComponent<GameController> ();
		startSpawnTime = spawnTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (waitUntil != null && !waited) {
			EnemySpawner waitSpawner = waitUntil.GetComponent<EnemySpawner> ();
			if (waitSpawner.spawnTime != waitSpawner.minSpawnTime)
				return;
			if (!waited) {
				waited = true;
				waitSpawner.spawnTime = waitSpawner.startSpawnTime;
			}
		}
		currentSpawnTime += Time.deltaTime;
		if (currentSpawnTime >= spawnTime) {
			currentSpawnTime -= spawnTime;
			spawnTime -= decreaseSpawnTime;
			spawnTime = Mathf.Max (minSpawnTime, spawnTime);
			Transform enemy = Instantiate (enemies [Random.Range (0, enemies.Length)], transform.position, Quaternion.identity).transform;
			// enemy.parent = transform;
			if (withHelmet)
				enemy.FindChild ("Helmet").gameObject.SetActive (true);
			if (withHoverboard)
				enemy.FindChild ("Hoverboard").gameObject.SetActive (true);

			Enemy entity = enemy.GetComponent<Enemy> ();
			entity.health = (float)health;
			entity.maxHealth = (float)health;
			entity.controller = controller;
			entity.dropGold = Random.Range (minDropGold, maxDropGold + 1);
			controller.enemies.Add (enemy.gameObject);
		}
	}
}
