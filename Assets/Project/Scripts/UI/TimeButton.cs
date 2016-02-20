using UnityEngine;
using System.Collections;

public class TimeButton : MonoBehaviour {

    public GameObject inputManager;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        InputHandler ih = inputManager.GetComponent<InputHandler>();
        ih.SetInputMode(InputHandler.INPUT_MODE.ABILITY_TIME);
    }
}
