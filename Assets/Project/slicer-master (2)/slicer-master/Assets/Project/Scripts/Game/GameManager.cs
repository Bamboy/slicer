using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
	private static GameManager gm;
	public static GameManager Instance
	{
		get{
			if( gm == null )
				Debug.LogError("No instance of GameManager was found in the scene!");

				return gm; 
		}
	}
	void Awake()
	{
		gm = this;
		DontDestroyOnLoad( this.gameObject );
	}

	[SerializeField]
	private int points = 0;
	public int Points
	{
		get{ return points; }
		set{
			points = value;
			//Do other stuff like UI Animations here
		}
	}
	public void AddPoints( int amount )
	{
		//If pointmultiplier is active, give more points.
		Points += amount; //* pointmultiplier
	}
	[SerializeField]
	private int lives = 3;
	public int Lives
	{
		get{ return lives; }
		set{
			lives = value;
			if( value <= 0 )
			{
				//End the game.

			}
			//Do other stuff like UI Animations here
		}
	}

	//TODO ability gauge
}
