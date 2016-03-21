using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Paused : MonoBehaviour {

    GameObject[] pausedObject;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        pausedObject = GameObject.FindGameObjectsWithTag("PausedShow");
        HideitPased();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowUpPased();
            }
            else if (Time.timeScale == 0)
            {
                Debug.Log("Good Good!");
                Time.timeScale = 1;
                HideitPased();
            }
        }
	}

    public void PauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowUpPased();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HideitPased();
        }
    }

public void ShowUpPased()
    {
        foreach (GameObject g in pausedObject)
        {
            g.SetActive(true);
        }
        
    }
public void HideitPased()
    {
        foreach (GameObject g in pausedObject)
        {
            g.SetActive(false);
        }
    }
public void LoadUpMainMenu(string MainMenu)
    {
        SceneManager.LoadScene(MainMenu);
    }

}
