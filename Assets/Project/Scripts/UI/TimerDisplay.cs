using UnityEngine;
using UnityEngine.UI;
using System.Collections;

[RequireComponent(typeof(Text))]
public class TimerDisplay : MonoBehaviour 
{
	Text t;
	void Awake()
	{
		t = GetComponent<Text>();
	}
	void Update()
	{
		switch( GameManager.GameMode )
		{
		case GameModes.Arcade:

			ArcadeTimer();

			break;
		default:
			Debug.LogWarning("Timers not implemented for other gamemodes. Destroying UI.");
			Destroy( this.gameObject );
			break;
		}

	}
	void ArcadeTimer()
	{
		if( GameManager.Instance.GameTime < 30f )
		{
			t.text = "Almost out of time! " + VectorExtras.RoundTo( GameManager.Instance.GameTime, 0.1f );
			t.color = Color.red;
		}
		else
		{
			t.text = "Time remaining: " + VectorExtras.RoundTo( GameManager.Instance.GameTime, 1f );
			t.color = Color.white;
		}
	}


}
