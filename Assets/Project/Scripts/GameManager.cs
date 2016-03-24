using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	
	#region Singleton Initialize
	private static GameManager instance;
	public static GameManager Instance {
		get {
			if (instance == null) {
				instance = FindObjectOfType<GameManager> ();
				if (instance == null) {
					GameObject obj = new GameObject ();
					obj.name = "Game Manager";
					instance = obj.AddComponent<GameManager> ();
				}
			}
			return instance;
		}
	}

	public virtual void Awake () {
		DontDestroyOnLoad (this.gameObject);
		if (instance == null) {
			instance = this as GameManager;
		} else {
			Destroy (gameObject);
		}
	}
	#endregion


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

		float flashColorIncrement = 1f;
		float currentColorFlashValue = 0f;
		#endregion


		#region Buttons
		public Button timeAbilityButton;
		public System.Action onTimeAbilityButton = delegate { };

		public Button spaceAbilityButton;
		public System.Action onSpaceAbilityButton = delegate { };
		#endregion

		bool laserAbilityIsOn = false;
		float laserAbilityStartTime = 0f;

		public GameObject[] objectsToSpawn;

		public Material lineMat;

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

			GameManager.Instance.StartCoroutine (SpawnObj ());
			GameManager.Instance.StartCoroutine (RunSlashing ());
		}

		public override void Update() {
			base.Update ();
			RunTimer ();
		}

		public IEnumerator RunSlashing() {
			bool firstClick = true;

			LineRenderer lineRenderer = new LineRenderer ();
			List<Vector3> pointList = new List<Vector3> (6);

			int pointIndex = 0;

			while (true) {
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
									Destroy (hitData.collider.gameObject);
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
					currentColorFlashValue += 0.05f * flashColorIncrement;

					if (flashColorIncrement > 0f && currentColorFlashValue >= 1f) {
						flashColorIncrement = -1f;
						currentColorFlashValue = 1f;
					} else if (flashColorIncrement < 0f && currentColorFlashValue <= 0f) {
						flashColorIncrement = 1f;
						currentColorFlashValue = 0f;
					}

					Color curColor = timerText.color;
					curColor.g = curColor.b = currentColorFlashValue;

					timerText.color = curColor;
				} else if (timerText.color != Color.white) {
					timerText.color = Color.white;
				}
			}
		}

		public IEnumerator SpawnObj() {
			while (true) {
				GameObject randomObj = objectsToSpawn [Random.Range (0, objectsToSpawn.Length)];
				GameObject newObj = Instantiate (randomObj);

				Vector3 screenPoint = new Vector3 (Random.Range (0, Screen.width), 0, 10f);
				newObj.transform.position = Camera.main.ScreenToWorldPoint (screenPoint);

				float xForce;
				if (screenPoint.x < Screen.width / 2f) {
					xForce = Random.Range (1f, 3f);
				} else {
					xForce = -Random.Range (1f, 3f);
				}

				newObj.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (xForce, Random.Range (8f, 12f)), ForceMode2D.Impulse);

				GameManager.Instance.StartCoroutine (slicerObjectUpdate (newObj));

				DestroyObject (newObj, 8f);

				yield return new WaitForSeconds (1.5f);
			}
		}

		public IEnumerator slicerObjectUpdate(GameObject obj) {
			float rotationValue = (Random.Range(0,2) * 2 - 1) * Mathf.Abs (Random.Range (100f, 300f));

			while (obj != null) {
				obj.transform.Rotate (0, 0, rotationValue * Time.deltaTime);

				Vector3 screenPoint = Camera.main.WorldToScreenPoint (obj.transform.position + Vector3.up * 2f);
				if (screenPoint.y < 0) {
					Destroy (obj);
				}
				yield return null;
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
						Destroy (hitData.collider.gameObject);
					}

					pointList [i] = pointList [i + 1];

					lineRenderer.SetPosition (i, pointList [i]);
				}



				yield return null;
			}

			DestroyObject (lineRenderObject);
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

/*

:: OLD ::

public enum GameModes
{
	MainMenu = 0,
	Classic = 1,
	Arcade = 2,
	Pitch = 3,
	Zen = 4
};

public class GameManager : MonoBehaviour 
{
    GameObject[] FinishedScript;
    
    // this is for the game ending and finished
    
    private static GameManager gm;
	public static GameManager Instance
	{
		get{
			if( gm == null )
				Debug.LogError("No instance of GameManager was found in the scene!");
			return gm;
		}
	}

	private static GameModes _gameMode = GameModes.MainMenu;
	public static GameModes GameMode
	{
		get{ return _gameMode; }
		set{
			GameManager.Instance.StopAllCoroutines(); //This may cause problems if non-timer coroutines are implemented.

			//Switch statement to reset values to defaults as if we started a new game of specified game type.
			//Possibly add Application.LoadLevel here, handle ending of a game?
			switch( value )
			{
			case GameModes.Classic:
				GameManager.Instance.GameTime = 0f;
				break;
			case GameModes.Arcade:
				GameManager.Instance.GameTime = 5f;//180f;
				GameManager.Instance.StartCoroutine("Timer_Arcade"); //Start the timer loop    
				break;
			case GameModes.Pitch:
				GameManager.Instance.GameTime = 0f;
				break;
			case GameModes.Zen:
				GameManager.Instance.GameTime = 0f;
				break;
			default: //Default act as main menu
				GameManager.Instance.GameTime = 0f;
				break;
			}

			//Reset general variables.
			GameManager.Instance.Points = 0;
			GameManager.Instance.Lives = 3;
			GameManager.Instance.AbilityMeter = 0f;


			//Set internal gamemode var
			_gameMode = value;
		}
	}

	void Awake()
	{
		if( gm != null )
			Destroy( this.gameObject );
		else
			gm = this;

		//TODO make the below dynamic based on loaded scene. We aren't worrying about other gamemodes yet so this is fine.
		GameMode = GameModes.Arcade;

		DontDestroyOnLoad( this.gameObject );

		FinishedScript = GameObject.FindGameObjectsWithTag("ShowFinished");
		hideFinished ();
	}


	#region Points
	[SerializeField]
	private int _points = 0;
	public int Points
	{
		get{ return _points; }
		set{
			_points = value;
			//Do other stuff like UI Animations here
		}
	}
	public void AddPoints( int amount )
	{
		//If pointmultiplier is active, give more points.
		Points += amount; //* pointmultiplier
	}
	#endregion


	#region Lives
	[SerializeField]
	private int _lives = 3;
	public int Lives
	{
		get{ return _lives; }
		set{
			_lives = value;
			if( value <= 0 )
			{
				//End the game.

			}
			//Do other stuff like UI Animations here
		}
	}
	#endregion


	#region AbilityMeter
	public AbilityMeterUI abilityMeterUI;
	[SerializeField]
	private float _abilityMeter = 0f;
	public float AbilityMeter
	{
		get{ return _abilityMeter; }
		set{
			_abilityMeter = Mathf.Clamp01(value);
			abilityMeterUI.OnMeterValueChanged(value); //Notify UI
		}
	}
	public void AddMeter( float percentage )
	{
		//If meter multiplier is active, give more meter.
		AbilityMeter += percentage; //* meterMultiplier;
	}

	//Is the ability meter full?
	public bool AbilityMeterIsFull() { return ( _abilityMeter == 1f ); }
	#endregion


	#region GameTimer
	public ObjectSpawner objectSpawner;
	public float _gameTime = 0f;
    
	public float GameTime
	{
       
		get{ return _gameTime; }
		set{
			//Do different things here based on game mode.
			switch( GameMode )
			{
			case GameModes.Arcade:
				if (value <= 0f) {
					//Time has run out, end the game.
					GameOver();
					Debug.LogWarning ("Game has run out of time.");
					//Debug.Break();
				} else if (!objectSpawner.isSpawning) {
					objectSpawner.isSpawning = true;
				}
				break;
			default:
				break;
			}
				
			_gameTime = value;
		}
	}

    //If Timer runs out the game ends there.
    public void GameOver()
    {
		objectSpawner.isSpawning = false;
		showFinished ();
		StopCoroutine ("Timer_Arcade");
    }

	public void OnRestart() {
		SceneManager.LoadScene ("Gameplay");
		//hideFinished ();
		//objectSpawner.isSpawning = true;
		//GameMode = GameModes.Arcade;
	}

	public void OnQuit() {
		SceneManager.LoadScene ("MainMenu");
		//hideFinished ();
		//GameMode = GameModes.MainMenu;
	}

    public void showFinished()
    {
        foreach (GameObject g in FinishedScript)
        {
            g.SetActive(true);
        }
    }
    public void hideFinished()
    {
        foreach (GameObject g in FinishedScript)
        {
            g.SetActive(false);
        }
    }

    IEnumerator Timer_Arcade()
	{
		while( true )
		{
		    GameTime = GameTime -= Time.deltaTime; //TODO - make our own delta time variable that will scale negatively

			yield return null;
		}
	}

	//TODO - default increasing timer.

	#endregion
}
*/