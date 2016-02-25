using UnityEngine;
using System.Collections;

public class SpaceButton : MonoBehaviour {

    public GameObject g;

    InputHandler ih;
    public FullSwingMeterScript fsmc;
	// Use this for initialization
	void Start () {
        ih = g.GetComponent<InputHandler>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Click()
    {
        if (fsmc.GetFillAmount() >= 1 && ih.GetInputMode() == InputHandler.INPUT_MODE.NORMAL)
        {
            ih.SetInputMode(InputHandler.INPUT_MODE.ABILITY_SPACE);
            fsmc.EmptyMeter();
        }
        else
        {
            //play sound or display text stating not enough charge
            Debug.Log("No way, jose");
        }
    }
}
