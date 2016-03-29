using UnityEngine;
using System.Collections;

public class AbilitiesManager : Singleton<AbilitiesManager> {

	bool laserAbilityIsOn = false;
	// When was laser ability first activated?
	float laserAbilityStartTime = 0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void RunTimeAbility() {
		if (PointsHandler.Instance.barPercentage >= 0.99f) {
			PointsHandler.Instance.SetBarPercentage (0f);
			GameManager.Instance.StartCoroutine (_RunTimeAbility ());
		}
	}

	IEnumerator _RunTimeAbility() {
		Time.timeScale = 0f;
		yield return Utilities.Coroutines.WaitForRealSeconds (5f);
		Time.timeScale = 1f;
	}

	public void NewLaser() {
		if (laserAbilityIsOn || PointsHandler.Instance.barPercentage >= 0.99f) {
			PointsHandler.Instance.SetBarPercentage (0f);
			GameManager.Instance.StartCoroutine (_NewLaser ());
		}
	}

	IEnumerator _NewLaser() {
		if (!laserAbilityIsOn) {
			laserAbilityIsOn = true;
			laserAbilityStartTime = Time.time;
		}

		LineRenderManager.LineRenderObject lineRenderer = LineRenderManager.Instance.AddLineRenderer (0.05f, 0.05f, Color.red, Color.red, 12, 5);
		lineRenderer.onCollision = delegate (GameObject obj) {
			obj.GetComponent<ISlicable>().onSliced();
		};

		// Start with a random point, anywhere on the screen
		Vector3 curPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 10f));
		curPoint.z = 0f;

		lineRenderer.NextPoint (curPoint);

		// Set a random laser direction
		Vector3 laserDirection = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0).normalized * 1f;
		laserDirection.z = 0f;

		// How many points are out of screen bounds?
		int outOfBoundsCount = 0;

		while (true) {
			Vector3 screenPoint = Camera.main.WorldToScreenPoint (curPoint);

			// Is point out of screen bounds horizontally?
			bool outOfScreenBoundsX = screenPoint.x < 0 || screenPoint.x > Screen.width;
			// Is point out of screen bounds vertically?
			bool outOfScreenBoundsY = screenPoint.y < 0 || screenPoint.y > Screen.height;

			// Is point out of screen bounds?
			bool outOfScreenBounds = outOfScreenBoundsX || outOfScreenBoundsY;

			// Did ability time run out?
			bool timeRanOut = Time.unscaledTime >= laserAbilityStartTime + 4f;

			if (!timeRanOut) {
				// If last point of laser is out of screen bounds, deflect it
				if (outOfScreenBoundsX) {
					laserDirection.x *= -1;
				}
				if (outOfScreenBoundsY) {
					laserDirection.y *= -1;
				}
			} else if (outOfScreenBounds) {
				// Point is out of bounds, increment number of points that are out of bounds
				outOfBoundsCount++;

				// When whole laser is out of bounds, destroy it
				if (outOfBoundsCount >= 11) {
					break;
				}
			}

			if ((timeRanOut && !outOfScreenBounds) || (!timeRanOut)) {

				// Used for randomly pushing laser a bit in random direction, making it look like lightning bolt
				//Vector3 noise = new Vector3 (Random.Range (0, 0), Random.Range (0, 0), 0);

				// Make the laser move forwards
				curPoint = curPoint + laserDirection; // + noise;
			}

			lineRenderer.NextPoint (curPoint);

			yield return null;
		}

		lineRenderer.DestroyRenderer ();
		laserAbilityIsOn = false;

		yield return null;
	}
}
