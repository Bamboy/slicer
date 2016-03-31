using UnityEngine;
using System.Collections;

public static class Utilities {

	public static class Coroutines {

		// Used when Time.timeScale != 0
		public static IEnumerator WaitForRealSeconds(float time) {
			float start = Time.realtimeSinceStartup;

			while (Time.realtimeSinceStartup < start + time) {
				yield return null;
			}
		}

	}

	public static class DateAndTime {

		// Format seconds to MM:SS
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

			public static float LerpNum(float start, float end, float t) {
				return (1 - t) * start + t * end;
			}

			// Lerp color between 2 values, using ping-pong effect
			public static Color LerpColorPingPong(Color startColor, Color endColor, float t) {
				t = Mathf.PingPong (t, 1.0f);

				float r1, g1, b1;
				float r2, g2, b2;

				r1 = startColor.r;
				g1 = startColor.g;
				b1 = startColor.b;

				r2 = endColor.r;
				g2 = endColor.g;
				b2 = endColor.b;

				float r, g, b;

				r = LerpNum (r1, r2, t);
				g = LerpNum (g1, g2, t);
				b = LerpNum (b1, b2, t);

				return new Color (r, g, b);
			}

		}

		public static class Noise {



		}

	}
}