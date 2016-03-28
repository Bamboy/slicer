using UnityEngine;
using System.Collections;

public sealed class ObjectSpawner : Singleton<ObjectSpawner> {

	public GameObject[] objectsToSpawn;
	public float spawnRate = 1f;

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

				GameObject randomObj = objectsToSpawn [Random.Range (0, objectsToSpawn.Length)];
				GameObject newObj = Instantiate (randomObj);

				Vector3 screenPoint = new Vector3 (Random.Range (0, Screen.width), 0, 10f);
				newObj.transform.position = Camera.main.ScreenToWorldPoint (screenPoint);

				float xForce;
				if (screenPoint.x < Screen.width / 2f) {
					xForce = Random.Range (1f, 3f);
				} else {
					xForce = -Random.Range (1f, 3f);
				}

				newObj.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (xForce, Random.Range (8f, 12f)), ForceMode2D.Impulse);
			}
		}
	}
}
