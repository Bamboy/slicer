using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ISlicable))]
public class HazardObject : MonoBehaviour {

	public void Start() {
		GetComponent<ISlicable> ().onSliced = delegate {
			if (GetComponent<ISlicable> ().sliceEnabled) {
				Camera.main.GetComponent<ShakeCamera>().Shake();

				TimeManager.Instance.timerSeconds -= 5f;
				PointsHandler.Instance.AddPoints(-50);
				PointsHandler.Instance.SetBarPercentage(0);

				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ISlicable")) {
					Destroy(obj);
				}

				Destroy (gameObject);
			}
		};
	}

}