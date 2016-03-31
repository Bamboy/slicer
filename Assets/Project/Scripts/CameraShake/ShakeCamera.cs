using UnityEngine;
using System.Collections;

public class ShakeCamera : MonoBehaviour {

	public float duration = 0.5f;
	public float speed = 1.0f;
	public float magnitude = 0.1f;

	public GameObject arcadeModeHolder;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.A)) {
			Shake ();
		}
	}

	public void Shake() {
		StopAllCoroutines ();
		StartCoroutine (_Shake ());
	}

	IEnumerator _Shake() {
		float elapsed = 0.0f;

		float camPosZ = Camera.main.transform.position.z;
		float arcadePosZ = arcadeModeHolder.transform.position.z;

		float randomStart = Random.Range(-1000.0f, 1000.0f);

		while (elapsed < duration) {
			elapsed += Time.unscaledDeltaTime;			

			float percentComplete = elapsed / duration;			

			// We want to reduce the shake from full power to 0 starting half way through
			float damper = 1.0f - Mathf.Clamp(2.0f * percentComplete - 1.0f, 0.0f, 1.0f);

			// Calculate the noise parameter starting randomly and going as fast as speed allows
			float alpha = randomStart + speed * percentComplete;

			// map noise to [-1, 1]
			float x = Util.Noise.GetNoise(alpha, 0.0f, 0.0f) * 2.0f - 1.0f;
			float y = Util.Noise.GetNoise(0.0f, alpha, 0.0f) * 2.0f - 1.0f;

			x *= magnitude * damper;
			y *= magnitude * damper;

			Camera.main.transform.position = new Vector3(x, y, camPosZ);
			arcadeModeHolder.transform.position = new Vector3(-x, -y, arcadePosZ);

			yield return null;
		}

		Camera.main.transform.position = new Vector3 (0, 0, camPosZ);
		arcadeModeHolder.transform.position = new Vector3 (0, 0, arcadePosZ);
	}
}