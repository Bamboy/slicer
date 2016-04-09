using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeManager : Singleton<TimeManager> {

	public float timerSeconds;
	public Text timerText;

	[HideInInspector]
	public bool active = false;

	float startTime = 0f;
	public float endTime {
		get { return startTime + timerSeconds; }
	}
	public float curTime {
		get { return Time.time - startTime; }
	}
	public float timeRemaining {
		get { return timerSeconds - curTime; }
	}

	public void StartTimer() {
		active = true;
		startTime = Time.time;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (active) {
			//Debug.Log ("timerSeconds: " + timerSeconds + "\nstartTime: " + startTime + "\nendTime: " + endTime + "\ncurTime: " + curTime + "\ntimeRemaining: " + timeRemaining);

			if (curTime > endTime) {
				timerText.text = "Time remaining: 00:00";
				timerText.color = Color.red;
			} else {
				// Turn seconds into format MM:SS
				string timeFormat = Utilities.DateAndTime.SecondsToTime (timeRemaining);
				timerText.text = "Time remaining: " + timeFormat;

				int secondsRemaining = Mathf.FloorToInt (timeRemaining);

				// Intervals when text should flash
				if ((secondsRemaining <= 30 && secondsRemaining >= 25 + 1) || (secondsRemaining <= 10)) {
					Color curColor = timerText.color;
					curColor = Utilities.Math.Lerp.LerpColorPingPong (Color.white, Color.red, timeRemaining * 5f);

					timerText.color = curColor;
				} else if (timerText.color != Color.white) {
					timerText.color = Color.white;
				}
			}
		}
	}
}
