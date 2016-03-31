using UnityEngine;
using System.Collections;

[RequireComponent(typeof(ISlicable))]
public class SlicerObject : MonoBehaviour {
	public int pointValue;

	public void Start() {
		GetComponent<ISlicable> ().onSliced = delegate {
			if (GetComponent<ISlicable> ().sliceEnabled) {
				Camera.main.GetComponent<ShakeCamera>().Shake();

				PointsHandler.Instance.AddPoints(pointValue);
				Destroy (gameObject);
			}
		};
	}

}