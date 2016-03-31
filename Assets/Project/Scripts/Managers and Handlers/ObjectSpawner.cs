using UnityEngine;
using System.Collections;

public sealed class ObjectSpawner : Singleton<ObjectSpawner> {

	public GameObject[] objectsToSpawn;
	// How long to wait until next object is spawned
	public float spawnRate = 1f;
	// When was the last object spawned
	float lastSpawnTime = 0f;

	[HideInInspector]
	public bool active = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			if (Time.time >= lastSpawnTime + spawnRate) {
				lastSpawnTime = Time.time;

				spawnObject ();

				spawnRate -= spawnRate / 200f;
			}
		}
	}

	public GameObject getRandomObject() {
		float weightSum = 0f;
		float[] weightArray = new float[objectsToSpawn.Length];

		for (int i = 0; i < objectsToSpawn.Length; i++) {
			float weight = objectsToSpawn [i].GetComponent<ISlicable> ().spawnProbability;
			weightSum += weight;
			weightArray [i] = weight;
		}

		GameObject randomObj = null;

		float randomNum = Random.Range (0f, weightSum);
		for (int i = 0; i < objectsToSpawn.Length; i++) {
			if (randomNum < weightArray [i]) {
				randomObj = objectsToSpawn [i];
				break;
			} else {
				randomNum -= weightArray [i];
			}
		}

		if (randomObj == null) {
			randomObj = objectsToSpawn [Random.Range (0, objectsToSpawn.Length)];
		}

		return randomObj;
	}

	public Vector3 getRandomPosition() {
		// Pick a random point on lower part of screen
		Vector3 screenPoint = new Vector3 (Random.Range (0, Screen.width), 0, 10f);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint (screenPoint);

		return objPosition;
	}

	public Vector2 getRandomForce(Vector3 objPos) {
		Vector3 screenPoint = Camera.main.WorldToScreenPoint (objPos);

		// If object is left, push right and vice versa
		float xForce;
		if (screenPoint.x < Screen.width / 2f) {
			xForce = Random.Range (1f, 3f);
		} else {
			xForce = -Random.Range (1f, 3f);
		}

		return new Vector2 (xForce, Random.Range (8f, 12f));
	}

	public GameObject spawnObject() {
		GameObject randomObj = getRandomObject ();
		Vector3 randomPos = getRandomPosition ();
		Vector2 randomForce = getRandomForce (randomPos);

		return spawnObject (randomObj, randomPos, randomForce);
	}

	public GameObject spawnObject(GameObject obj) {
		Vector3 randomPos = getRandomPosition ();
		Vector2 randomForce = getRandomForce (randomPos);

		return spawnObject (obj, randomPos, randomForce);
	}

	public GameObject spawnObject(Vector3 pos) {
		GameObject randomObj = getRandomObject ();
		Vector2 randomForce = getRandomForce (pos);

		return spawnObject (randomObj, pos, randomForce);
	}

	public GameObject spawnObject(Vector2 force) {
		GameObject randomObj = getRandomObject ();
		Vector3 randomPos = getRandomPosition ();

		return spawnObject (randomObj, randomPos, force);
	}

	public GameObject spawnObject(GameObject obj, Vector2 force) {
		Vector3 randomPos = getRandomPosition ();
		return spawnObject (obj, randomPos, force);
	}

	public GameObject spawnObject(Vector3 pos, Vector2 force) {
		GameObject randomObj = getRandomObject ();
		return spawnObject (randomObj, pos, force);
	}

	public GameObject spawnObject(GameObject obj, Vector3 pos, Vector2 force) {
		GameObject newObj = Instantiate (obj);

		newObj.transform.position = pos;
		newObj.GetComponent<Rigidbody2D> ().AddForce (force, ForceMode2D.Impulse);

		return newObj;
	}
}
