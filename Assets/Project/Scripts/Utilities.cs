using UnityEngine;
using System.Collections;

public static class Utilities {

	public static class Coroutines {
		
		public static IEnumerator WaitForRealSeconds(float time) {
			float start = Time.realtimeSinceStartup;

			while (Time.realtimeSinceStartup < start + time) {
				yield return null;
			}
		}

	}

	public static class DateAndTime {
		
		public static string SecondsToTime(float floatSeconds) {
			int seconds = Mathf.FloorToInt (floatSeconds);
			int minutes = seconds / 60;
			seconds -= minutes * 60;

			string secStr = seconds.ToString ();
			string minStr = minutes.ToString ();

			if (seconds < 10) {
				secStr = "0" + secStr;
			}
			if (minutes < 10) {
				minStr = "0" + minStr;
			}

			return minStr + ":" + secStr;
		}

	}

	public static class Math {
		
		public static class Lerp {



		}

	}
}
