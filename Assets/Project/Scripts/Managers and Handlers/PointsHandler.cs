using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointsHandler : Singleton<PointsHandler> {

	public Text pointsText;
	public Text pointsTextShadow;

	public Image pointsBar;

	int points = 0;
	public float barPercentage = 0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void AddPoints(int value) {
		if (points + value < 0) {
			SetPoints (0, -points);
		} else {
			SetPoints (points + value, value);
		}
	}

	public void SetPoints(int value, int increment) {
		points = value;
		pointsText.text = "Points: " + points.ToString ();
		pointsTextShadow.text = "Points: " + points.ToString ();

		SetBarPercentage (barPercentage + increment / 100f);
	}

	public void SetBarPercentage(float value) {
		barPercentage = Mathf.Clamp01 (value);
		pointsBar.fillAmount = barPercentage;
	}
}
