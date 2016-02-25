using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FullSwingMeterScript : MonoBehaviour {

    float fillAmount;

    public Image meterFill;

	// Use this for initialization
	void Start () {
        fillAmount = 0;
        meterFill.fillAmount = fillAmount;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public float GetFillAmount()
    {
        return fillAmount;
    }

    //Will increase the fill meter by given value. Percent value between 0 and 1
    public void AddToMeter(float _percentToFill)
    {
        fillAmount += _percentToFill;
        
        if (fillAmount > 1) fillAmount = 1;

        meterFill.fillAmount = fillAmount;
    }

    //Called on ability use
    public void EmptyMeter()
    {
        fillAmount = 0;
        meterFill.fillAmount = 0;
    }


}
