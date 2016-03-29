using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public sealed class LineRenderManager : Singleton<LineRenderManager> {

	public Material lineMaterial;

	public class LineRenderObject {
		LineRenderer lineRenderer;

		List<Vector3> pointList;
		int pointCount;

		// Number of points on the end of line that will detect collision
		int collisionPoints;
		// Are points added for the first time?
		bool firstPoint = true;

		public System.Action<GameObject> onCollision = delegate { };

		public LineRenderObject(LineRenderer lineRenderer, int pointCount, int collisionPoints) {
			this.lineRenderer = lineRenderer;
			this.pointCount = pointCount;

			// Number of points that detect collision can't be greater than number of points
			if (collisionPoints > pointCount) {
				collisionPoints = pointCount;
			}
			this.collisionPoints = collisionPoints;

			pointList = new List<Vector3>(pointCount);

			this.Clear();
		}

		public void NextPoint(Vector3 point) {
			for (int i = 0; i < pointCount; i++) {
				if (firstPoint) {
					pointList.Add (point);
					lineRenderer.SetPosition (i, point);
				} else if (i + 1 < pointCount) {
					// Don't update the last point, that is done after this loop

					// If current point can detect collision
					if (i >= pointCount - collisionPoints) {
						RaycastHit2D hitData = Physics2D.Linecast(pointList [i], pointList [i + 1]);

						if (hitData.collider != null) {
							GameObject hitObject = hitData.collider.gameObject;
							onCollision (hitObject);
						}
					}

					// Move all points forward
					pointList [i] = pointList [i + 1];
					lineRenderer.SetPosition (i, pointList [i]);
				}
			}

			// Update the last point
			int lastIndex = pointCount - 1;

			pointList [lastIndex] = point;
			lineRenderer.SetPosition (lastIndex, pointList [lastIndex]);

			if (firstPoint) {
				firstPoint = false;
			}
		}

		public void Clear() {
			pointList.Clear ();
			firstPoint = true;

			lineRenderer.SetVertexCount (pointCount);

			for (int i = 0; i < pointCount; i++) {
				lineRenderer.SetPosition (i, Vector3.zero);
			}
		}

		public void DestroyRenderer() {
			Destroy (lineRenderer.gameObject);
		}
	}

	public LineRenderObject AddLineRenderer(float startWidth, float endWidth, Color startColor, Color endColor, int pointCount, int collisionPoints) {
		GameObject lineRenderObject = new GameObject ("Line Renderer");
		lineRenderObject.transform.parent = transform;

		LineRenderer lineRenderer = lineRenderObject.AddComponent<LineRenderer> ();
		lineRenderer.SetWidth (startWidth, endWidth);
		lineRenderer.SetColors (startColor, endColor);
		lineRenderer.SetVertexCount (pointCount);

		lineRenderer.sharedMaterial = lineMaterial;

		return new LineRenderObject (lineRenderer, pointCount, collisionPoints);
	}

}
