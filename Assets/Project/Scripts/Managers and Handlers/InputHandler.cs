using UnityEngine;
using System.Collections;

public class InputHandler : Singleton<InputHandler> {

	bool isHoldingMouseDown = false;
	LineRenderManager.LineRenderObject lineRenderer;

	[HideInInspector]
	public bool active = false;

	// Use this for initialization
	void Start () {
		lineRenderer = LineRenderManager.Instance.AddLineRenderer (0.01f, 0.2f, new Color (1, 1, 1, 0), Color.white, 24, 5);
		lineRenderer.onCollision = delegate (GameObject obj) {
			int objPoints = int.Parse (obj.name.Substring (14, 1));

			PointsHandler.Instance.AddPoints(objPoints);

			Destroy (obj);
		};	
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			if (Input.GetMouseButton (0)) {
				isHoldingMouseDown = true;

				Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				mousePos.z = 0;

				lineRenderer.NextPoint (mousePos);
			} else {
				// When mouse button is released
				if (isHoldingMouseDown) {
					isHoldingMouseDown = false;
					lineRenderer.Clear ();
				}
			}
		}
	}
}