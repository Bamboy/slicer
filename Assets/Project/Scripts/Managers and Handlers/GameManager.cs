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
		#endregion

		public ArcadeMode() : base("Arcade") { }

		public override void Initialize() {
			base.Initialize ();
			// Assign methods to button's onClick event
			BindButton (timeAbilityButton, onTimeAbilityButton);
			BindButton (spaceAbilityButton, onSpaceAbilityButton);
		}

		public override void Activate() {
			base.Activate ();

			ObjectSpawner.Instance.active = true;
			InputHandler.Instance.active = true;
			TimeManager.Instance.StartTimer ();
		}

		public override void Update() {
			base.Update ();
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
