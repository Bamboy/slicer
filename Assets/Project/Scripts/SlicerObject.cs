using UnityEngine;
using System.Collections;

public class SlicerObject : MonoBehaviour {

	float rotationValue;

	// Use this for initialization
	void Start () {
		rotationValue = (Random.Range(0,2) * 2 - 1) * Mathf.Abs (Random.Range (100f, 300f));
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotationValue * Time.deltaTime);

		Vector3 screenPoint = Camera.main.WorldToScreenPoint (transform.position + Vector3.up * 2f);
		if (screenPoint.y < 0) {
			Destroy (gameObject);
		}
	}
}