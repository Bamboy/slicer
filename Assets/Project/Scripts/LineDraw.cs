using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineDraw : MonoBehaviour {

	#region Singleton Initialize
	private static LineDraw instance;
	public static LineDraw Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<LineDraw> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
					obj.name = "Line Renderer";
					instance = obj.AddComponent<LineDraw> ();
				}
			}
			return instance;
		}
	}

	public virtual void Awake () {
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this as LineDraw;
		} else {
			Destroy (gameObject);
		}
	}
	#endregion

	public Material lineMaterial;

	public class LineRenderObject {
		LineRenderer lineRenderer;

		List<Vector3> pointList;
		int pointCount;

		bool firstPoint = true;

		public LineRenderObject(LineRenderer lineRenderer, int pointCount) {
			this.lineRenderer = lineRenderer;
			this.pointCount = pointCount;

			pointList = new List<Vector3>(pointCount);
		}

		public void NextPoint(Vector3 point) {
			for (int i = 0; i < pointCount; i++) {
				if (firstPoint) {
					pointList.Add (point);
					lineRenderer.SetPosition (i, point);
				} else {
					pointList [i] = pointList [i + 1];
					lineRenderer.SetPosition (i, pointList [i]);
				}
			}

			int lastIndex = pointCount - 1;

			pointList [lastIndex] = point;
			lineRenderer.SetPosition (lastIndex, pointList [lastIndex]);

			if (firstPoint) {
				firstPoint = false;
			}
		}

		public void DestroyRenderer() {
			Destroy (lineRenderer.gameObject);
		}
	}

	public LineRenderObject AddLineRenderer(float startWidth, float endWidth, Color startColor, Color endColor, int pointCount) {
		GameObject lineRenderObject = new GameObject ("Line Renderer");
		lineRenderObject.transform.parent = transform;

		LineRenderer lineRenderer = lineRenderObject.AddComponent<LineRenderer> ();
		lineRenderer.SetWidth (startWidth, endWidth);
		lineRenderer.SetColors (startColor, endColor);
		lineRenderer.SetVertexCount (pointCount);

		lineRenderer.sharedMaterial = lineMaterial;

		return new LineRenderObject (lineRenderer, pointCount);
	}

}

/*

if (Input.GetMouseButton (0)) {
	if (firstClick) {
		firstClick = false;

		if (pointList != null) {
			pointList = new List<Vector3> (24);
		}
		if (lineRenderer != null) {
			Destroy (lineRenderer.gameObject);
		}

		GameObject lineRenderObject = new GameObject ("Line Renderer");
		lineRenderer = lineRenderObject.AddComponent<LineRenderer> ();

		lineRenderer.SetWidth (0.01f, 0.2f);
		lineRenderer.SetColors (new Color (1, 1, 1, 0), Color.white);
		lineRenderer.SetVertexCount (24);

		lineRenderer.sharedMaterial = lineMat;

		for (int i = 0; i < 24; i++) {
			Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			mousePos.z = 0;

			pointList.Add (mousePos);
			lineRenderer.SetPosition (i, mousePos);
		}
	} else {
		Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
		mousePos.z = 0;

		pointList [pointList.Count - 1] = mousePos;
		lineRenderer.SetPosition (pointList.Count - 1, pointList [pointList.Count - 1]);

		for (int i = 0; i < 24 - 1; i++) {
			if (i >= 20) {
				RaycastHit2D hitData = Physics2D.Linecast(pointList [i], pointList [i + 1]);
				if (hitData.collider != null) {
					GameObject hitObject = hitData.collider.gameObject;

					int objPoints = int.Parse (hitObject.name.Substring (14, 1));
					points += objPoints;
					pointsText.text = "Points: " + points.ToString ();
					pointsTextShadow.text = "Points: " + points.ToString ();

					pointsBar.fillAmount = Mathf.Min ((points / 100f), 1f);

					Destroy (hitObject);
				}
			}

			pointList [i] = pointList [i + 1];
			lineRenderer.SetPosition (i, pointList [i]);
		}
	}
} else {
	if (lineRenderer != null) {
		Destroy (lineRenderer.gameObject);
	}

	if (!firstClick) {
		firstClick = true;
	}
}

=================================================================

if (!laserAbilityIsOn) {
	laserAbilityIsOn = true;
	laserAbilityStartTime = Time.realtimeSinceStartup;
}

GameObject lineRenderObject = new GameObject ("Line Renderer");
LineRenderer lineRenderer = lineRenderObject.AddComponent<LineRenderer> ();

lineRenderer.SetWidth(0.05f, 0.05f);
lineRenderer.SetVertexCount(12);

List<Vector3> pointList = new List<Vector3> ();

Vector3 point = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 10f));

for (int i = 0; i < 12; i++) {
	pointList.Add (new Vector3 (point.x, point.y, 0));
}

Vector3 laserDirection = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0).normalized * 1f;
laserDirection.z = 0f;

int outOfBoundsCount = 0;

while (true) {
	Vector3 lastPoint = pointList [pointList.Count - 1];
	Vector3 screenPoint = Camera.main.WorldToScreenPoint (lastPoint);

	bool outOfScreenBoundsX = screenPoint.x < 0 || screenPoint.x > Screen.width;
	bool outOfScreenBoundsY = screenPoint.y < 0 || screenPoint.y > Screen.height;

	bool outOfScreenBounds = outOfScreenBoundsX || outOfScreenBoundsY;
	bool timeRanOut = Time.realtimeSinceStartup >= laserAbilityStartTime + 4f;
	
	if (!timeRanOut) {
		if (outOfScreenBoundsX) {
			laserDirection.x *= -1;
		}
		if (outOfScreenBoundsY) {
			laserDirection.y *= -1;
		}
	} else if (outOfScreenBounds) {
		outOfBoundsCount++;

		if (outOfBoundsCount >= 11) {
			break;
		}
	}

	if ((timeRanOut && !outOfScreenBounds) || (!timeRanOut)) {
		Vector3 noise = new Vector3 (Random.Range (0, 0), Random.Range (0, 0), 0);
		pointList [pointList.Count - 1] = new Vector3 (lastPoint.x, lastPoint.y, 0) + laserDirection + noise;
		lineRenderer.SetPosition (pointList.Count - 1, pointList [pointList.Count - 1]);
	}

	for (int i = 0; i < pointList.Count - 1; i++) {
		RaycastHit2D hitData = Physics2D.Linecast(pointList [i], pointList [i + 1]);
		if (hitData.collider != null) {
			GameObject hitObject = hitData.collider.gameObject;

			int objPoints = int.Parse (hitObject.name.Substring (14, 1));
			points += objPoints;
			pointsText.text = "Points: " + points.ToString ();
			pointsTextShadow.text = "Points: " + points.ToString ();

			pointsBar.fillAmount = Mathf.Min ((points / 100f), 1f);

			Destroy (hitObject);
		}

		pointList [i] = pointList [i + 1];

		lineRenderer.SetPosition (i, pointList [i]);
	}



	yield return null;
}

DestroyObject (lineRenderObject);
laserAbilityIsOn = false;

yield return null;

*/