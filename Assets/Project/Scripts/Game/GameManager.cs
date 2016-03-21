using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

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
                GameManager.Instance.GameTime = 9f;
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
   
	public float _gameTime = 0f;
    
	public float GameTime
	{
       
		get{ return _gameTime; }
		set{
			//Do different things here based on game mode.
			switch( GameMode )
			{
			case GameModes.Arcade:
				if( value <= 0f )
				{
                        showFinished();
     //                   //Time has run out, end the game.
     //               GameOver();
					StopCoroutine("Timer_Arcade");
					Debug.LogWarning("Game has run out of time.");
					Debug.Break();
				}
                    
				break;
			default:
				break;
			}
				
			_gameTime = value;
		}
	}

    ////If Timer runs out the game ends there.
    //public void GameOver()
    //{

    //}
    public void showFinished()
    {
        FinishedScript = GameObject.FindGameObjectsWithTag("ShowFinished");
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
