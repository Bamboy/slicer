using UnityEngine;
using System.Collections;

public class ISlicable : MonoBehaviour {

	// How much the object will rotate
	float rotationValue;

	public bool sliceEnabled = true;

	public System.Action onSliced = delegate { };

	// Use this for initialization
	void Start () {
		rotationValue = (Random.Range(0,2) * 2 - 1) * Mathf.Abs (Random.Range (100f, 300f));
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate (0, 0, rotationValue * Time.deltaTime);

		Vector3 screenPoint = Camera.main.WorldToScreenPoint (transform.position + Vector3.up * 2f);

		// Is object out of lower screen bound?
		if (screenPoint.y < 0) {
			Destroy (gameObject);
		}
	}

}
