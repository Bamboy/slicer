using UnityEngine;
using System.Collections;

public class SpaceButton : MonoBehaviour {

    public GameObject g;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        g.GetComponent<InputHandler>().SetInputMode(InputHandler.INPUT_MODE.ABILITY_SPACE);  
    }
}
