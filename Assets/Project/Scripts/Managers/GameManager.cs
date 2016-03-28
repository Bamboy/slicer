using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : Singleton<GameManager> {

	#region Game Modes
	[System.Serializable]
	public class MainMenu : GameMode {
		#region Buttons
		public Button arcadeModeButton;
		public System.Action onArcadeButton = delegate { };

		public Button optionsButton;
		public System.Action onOptionsButton = delegate { };

		public Button creditsButton;
		public System.Action onCreditsButton = delegate { };

		public Button quitButton;
		public System.Action onQuitButton = delegate { };
		#endregion

		public MainMenu() : base("Main Menu") { }

		public override void Initialize() {
			base.Initialize ();
			// Assign methods to button's onClick event
			BindButton (arcadeModeButton, onArcadeButton);
			BindButton (optionsButton, onOptionsButton);
			BindButton (creditsButton, onCreditsButton);
			BindButton (quitButton, onQuitButton);
		}
	}


	[System.Serializable]
	public class ClassicMode : GameMode {
		public ClassicMode() : base("Classic") { }

		public override void Initialize() {
			base.Initialize ();
		}
	}
	
	[System.Serializable]
	public class ArcadeMode : GameMode {
		#region Timer variables
		public Text timerText;

		public float timerSeconds = 180f;
		float timeRemaining = 0f;
		float startTime = 0f;
		float curTime = 0f;
		float endTime = 0f;
		#endregion


		#region Buttons
		public Button timeAbilityButton;
		public System.Action onTimeAbilityButton = delegate { };

		public Button spaceAbilityButton;
		public System.Action onSpaceAbilityButton = delegate { };
		#endregion

		bool laserAbilityIsOn = false;
		float laserAbilityStartTime = 0f;

		public Text pointsText;
		public Text pointsTextShadow;

		public Image pointsBar;

		int points = 0;

		public ArcadeMode() : base("Arcade") { }

		public override void Initialize() {
			base.Initialize ();
			// Assign methods to button's onClick event
			BindButton (timeAbilityButton, onTimeAbilityButton);
			BindButton (spaceAbilityButton, onSpaceAbilityButton);
		}

		public override void Activate() {
			base.Activate ();
			startTime = Time.time;
			endTime = startTime + timerSeconds;

			ObjectSpawner.Instance.active = true;
			GameManager.Instance.StartCoroutine (RunSlashing ());
		}

		public override void Update() {
			base.Update ();
			RunTimer ();
		}

		public IEnumerator RunSlashing() {
			bool firstClick = true;

			LineRenderManager.LineRenderObject lineRenderer = LineRenderManager.Instance.AddLineRenderer (0.01f, 0.2f, new Color (1, 1, 1, 0), Color.white, 24, 5);
			lineRenderer.onCollision = delegate (GameObject obj) {
				int objPoints = int.Parse (obj.name.Substring (14, 1));
				points += objPoints;
				pointsText.text = "Points: " + points.ToString ();
				pointsTextShadow.text = "Points: " + points.ToString ();

				pointsBar.fillAmount = Mathf.Min ((points / 100f), 1f);

				Destroy (obj);
			};

			while (true) {
				if (Input.GetMouseButton (0)) {
					Vector3 mousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
					mousePos.z = 0;

					lineRenderer.NextPoint (mousePos);

					if (firstClick) {
						firstClick = false;
					}
				} else {
					lineRenderer.Clear ();

					if (!firstClick) {
						firstClick = true;
					}
				}

				yield return null;
			}
		}

		public void RunTimer() {
			curTime = Time.time - startTime;
			timeRemaining = endTime - curTime;

			if (curTime > timerSeconds) {
				timerText.text = "Time remaining: 00:00";
				timerText.color = Color.red;
			} else {
				string timeFormat = Utilities.DateAndTime.SecondsToTime (timeRemaining);
				timerText.text = "Time remaining: " + timeFormat;

				int secondsRemaining = Mathf.FloorToInt (timeRemaining);
				if ((secondsRemaining <= 30 && secondsRemaining >= 25 + 1) || (secondsRemaining <= 10)) {
					Color curColor = timerText.color;
					curColor = Utilities.Math.Lerp.LerpColorPingPong(Color.white, Color.red, timeRemaining * 5f);

					timerText.color = curColor;
				} else if (timerText.color != Color.white) {
					timerText.color = Color.white;
				}
			}
		}

		public IEnumerator TimeAbility() {
			Time.timeScale = 0f;
			yield return Utilities.Coroutines.WaitForRealSeconds (5f);
			Time.timeScale = 1f;
		}

		public IEnumerator SpaceAbility() {
			if (!laserAbilityIsOn) {
				laserAbilityIsOn = true;
				laserAbilityStartTime = Time.realtimeSinceStartup;
			}

			LineRenderManager.LineRenderObject lineRenderer = LineRenderManager.Instance.AddLineRenderer (0.05f, 0.05f, Color.red, Color.red, 12, 5);
			lineRenderer.onCollision = delegate (GameObject obj) {
				int objPoints = int.Parse (obj.name.Substring (14, 1));
				points += objPoints;
				pointsText.text = "Points: " + points.ToString ();
				pointsTextShadow.text = "Points: " + points.ToString ();

				pointsBar.fillAmount = Mathf.Min ((points / 100f), 1f);

				Destroy (obj);
			};

			Vector3 curPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Random.Range (0, Screen.width), Random.Range (0, Screen.height), 10f));
			curPoint.z = 0f;

			lineRenderer.NextPoint (curPoint);

			Vector3 laserDirection = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), 0).normalized * 1f;
			laserDirection.z = 0f;

			int outOfBoundsCount = 0;

			while (true) {
				Vector3 screenPoint = Camera.main.WorldToScreenPoint (curPoint);

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

					curPoint = curPoint + laserDirection + noise;
				}

				lineRenderer.NextPoint (curPoint);

				yield return null;
			}

			lineRenderer.DestroyRenderer ();
			laserAbilityIsOn = false;

			yield return null;
		}
	}


	[System.Serializable]
	public class PitchMode : GameMode {
		public PitchMode() : base("Pitch") { }

		public override void Initialize() {
			base.Initialize ();
		}
	}


	[System.Serializable]
	public class ZenMode : GameMode {
		public ZenMode() : base("Zen") { }

		public override void Initialize() {
			base.Initialize ();
		}
	}
	#endregion
	
	
	[System.Serializable]
	public class GameModes {
		public MainMenu mainMenu = new MainMenu ();
		public ClassicMode classicMode = new ClassicMode ();
		public ArcadeMode arcadeMode = new ArcadeMode ();
		public PitchMode pitchMode = new PitchMode ();
		public ZenMode zenMode = new ZenMode ();

		public void Initialize() {
			mainMenu.Initialize ();
			classicMode.Initialize ();
			arcadeMode.Initialize ();
			pitchMode.Initialize ();
			zenMode.Initialize ();
		}
	}

	public GameModes gameModes = new GameModes ();

	GameMode currentGameMode;

	public void Start() {
		gameModes.mainMenu.onArcadeButton = delegate {
			SetMode(gameModes.arcadeMode);
		};

		gameModes.arcadeMode.onTimeAbilityButton = delegate {
			StartCoroutine(gameModes.arcadeMode.TimeAbility());
		};

		gameModes.arcadeMode.onSpaceAbilityButton = delegate {
			StartCoroutine(gameModes.arcadeMode.SpaceAbility());
		};

		gameModes.Initialize ();

		SetMode (gameModes.mainMenu);
	}

	public void Update() {
		if (currentGameMode != null) {
			currentGameMode.Update ();
		}
	}

	public void SetMode(GameMode newMode) {
		if (currentGameMode != null) {
			currentGameMode.Deactivate ();
		}

		currentGameMode = newMode;

		currentGameMode.Activate ();
	}
}
