using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ISlicable))]
public class FreefallPowerUp : MonoBehaviour {

	public void Start() {
		GetComponent<ISlicable> ().onSliced = delegate {
			if (GetComponent<ISlicable> ().sliceEnabled) {
				Camera.main.GetComponent<ShakeCamera>().Shake();

				GameManager.Instance.StartCoroutine(spawnObjects());

				Destroy (gameObject);
			}
		};
	}

	IEnumerator spawnObjects() {
		float startTime = Time.time;

		while (Time.unscaledTime - startTime <= 6f) {
			float screenPosX;
			if (Random.Range (0, 100) < 50) {
				screenPosX = Random.Range (0, 20);
			} else {
				screenPosX = Random.Range (Screen.width - 20, Screen.width);
			}

			Vector3 screenPos = new Vector3 (screenPosX, Screen.height, 10);

			Vector3 objPos = Camera.main.ScreenToWorldPoint (screenPos);

			float xForce;
			if (screenPos.x < Screen.width / 2f) {
				xForce = Random.Range (1f, 6f);
			} else {
				xForce = -Random.Range (1f, 6f);
			}

			Vector2 force = new Vector2 (xForce, Random.Range (-1f, 3f));

			ObjectSpawner.Instance.spawnObject (objPos, force);

			yield return new WaitForSeconds (0.4f);
		}
	}

}

/*

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

*/