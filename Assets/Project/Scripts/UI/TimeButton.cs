using UnityEngine;
using System.Collections;

public class TimeButton : MonoBehaviour {

    public GameObject inputManager;

    InputHandler ih;

	// Use this for initialization
	void Start () {
        ih = inputManager.GetComponent<InputHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
		if (GameManager.Instance.AbilityMeterIsFull() && ih.GetInputMode() == InputHandler.INPUT_MODE.NORMAL)
        {
            ih.SetInputMode(InputHandler.INPUT_MODE.ABILITY_TIME);
			GameManager.Instance.AbilityMeter = 0f;
        }
        else
        {
            //play sound or display text stating not enough charge
            Debug.Log("No way, jose");
        }
    }
}
