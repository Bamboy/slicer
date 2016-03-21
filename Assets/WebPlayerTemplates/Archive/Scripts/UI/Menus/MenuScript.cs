using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour 
{

	public void StartMode(int mode)
	{
		switch( mode )
		{
		case 1:
			GameManager.GameMode = GameModes.Classic;
			break;
		case 2:
			GameManager.GameMode = GameModes.Arcade;
			break;
		case 3:
			GameManager.GameMode = GameModes.Pitch;
			break;
		case 4:
			GameManager.GameMode = GameModes.Zen;
			break;
		default: //Default act as main menu
			break;
		}

		SceneManager.LoadScene(1);
	}




    public void QuitGame()
    {
        //TODO
    }
}
