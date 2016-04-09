using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public sealed class GameManager : Singleton<GameManager> {

	#region Game Modes
	[System.Serializable]
	public class MainMenu : GameMode {
		#region Buttons
		// Set buttons and methods that are called when they are clicked

		public Button arcadeModeButton;
		public System.Action onArcadeButton = delegate { };

		public Button optionsButton;
		public System.Action onOptionsButton = delegate { };

		public Button creditsButton;
		public System.Action onCreditsButton = delegate { };

		public Button quitButton;
		public System.Action onQuitButton = delegate { };
		#endregion

		// Call the base constructor
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
		#region Buttons
		public Button timeAbilityButton;
		public System.Action onTimeAbilityButton = delegate { };

		public Button spaceAbilityButton;
		public System.Action onSpaceAbilityButton = delegate { };

		public Button restartButton;
		public System.Action onRestartButton = delegate { };
		public Button gameOverQuitButton;
		public System.Action onGameOverQuitButton = delegate { };

		public Button resumeButton;
		public System.Action onResumeButton = delegate { };
		public Button pauseQuitButton;
		public System.Action onPauseQuitButton = delegate { };
		#endregion

		public GameObject gameOverUI;
		public GameObject pauseGameUI;

		bool gameOver = false;

		public ArcadeMode() : base("Arcade") { }

		public override void Initialize() {
			base.Initialize ();
			// Assign methods to button's onClick event
			BindButton (timeAbilityButton, onTimeAbilityButton);
			BindButton (spaceAbilityButton, onSpaceAbilityButton);

			BindButton (restartButton, onRestartButton);
			BindButton (gameOverQuitButton, onGameOverQuitButton);

			BindButton (resumeButton, onResumeButton);
			BindButton (pauseQuitButton, onPauseQuitButton);
		}

		public override void Activate() {
			base.Activate ();

			gameOver = false;

			ObjectSpawner.Instance.spawnRate = 1.5f;
			TimeManager.Instance.timerSeconds = 180f;

			ObjectSpawner.Instance.active = true;
			InputHandler.Instance.active = true;
			TimeManager.Instance.StartTimer ();
		}

		public override void Update() {
			base.Update ();

			if (!gameOver && TimeManager.Instance.timeRemaining <= 1) {
				gameOver = true;

				ObjectSpawner.Instance.active = false;
				InputHandler.Instance.active = false;
				TimeManager.Instance.active = false;
				InputHandler.Instance.lineRenderer.DestroyRenderer ();

				PointsHandler.Instance.SetPoints (0, 0);
				PointsHandler.Instance.SetBarPercentage (0);

				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ISlicable")) {
					Destroy (obj);
				}

				gameOverUI.SetActive (true);
			}
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
			AbilitiesManager.Instance.RunTimeAbility();
		};
		gameModes.arcadeMode.onSpaceAbilityButton = delegate {
			AbilitiesManager.Instance.NewLaser();
		};
		gameModes.arcadeMode.onRestartButton = delegate {
			gameModes.arcadeMode.gameOverUI.SetActive(false);
			SetMode(gameModes.arcadeMode);
		};
		gameModes.arcadeMode.onGameOverQuitButton = delegate {
			gameModes.arcadeMode.gameOverUI.SetActive(false);
			SetMode(gameModes.mainMenu);
		};
		gameModes.arcadeMode.onResumeButton = delegate {

		};
		gameModes.arcadeMode.onPauseQuitButton = delegate {
			
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
