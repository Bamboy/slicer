using UnityEngine;
using System.Collections;

public class GameOver : GameManager {
    GameObject[] FinishedObject;
    
    // Use this for initialization
	public void StartFinish () {
        FinishedObject = GameObject.FindGameObjectsWithTag("ShowFinished");

        
	}

    //Time runs out
    public void TimeRunsout()
    {
        
    }
    
  
}
