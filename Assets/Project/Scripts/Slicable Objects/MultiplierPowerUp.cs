using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ISlicable))]
public class MultiplierPowerUp : MonoBehaviour {

	public void Start() {
		GetComponent<ISlicable> ().onSliced = delegate {
			if (GetComponent<ISlicable> ().sliceEnabled) {
				Camera.main.GetComponent<ShakeCamera>().Shake();

				Vector2 force1 = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 10f));
				Vector2 force2 = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 10f));

				GameObject newObj1 = ObjectSpawner.Instance.spawnObject(transform.position, force1);
				GameObject newObj2 = ObjectSpawner.Instance.spawnObject(transform.position, force2);

				GameManager.Instance.StartCoroutine(delaySlice(newObj1));
				GameManager.Instance.StartCoroutine(delaySlice(newObj2));

				Destroy (gameObject);
			}
		};
	}

	IEnumerator delaySlice(GameObject obj) {
		obj.GetComponent<ISlicable> ().sliceEnabled = false;
		yield return Utilities.Coroutines.WaitForRealSeconds (0.3f);
		if (obj != null) {
			obj.GetComponent<ISlicable> ().sliceEnabled = true;
		}
	}

}