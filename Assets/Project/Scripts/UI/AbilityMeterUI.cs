using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityMeterUI : MonoBehaviour {

	//Stephan Ennen 2/26 - 
	//Fill amount moved to GameManger. (See AbilityMeter) This script will be notifed of changes in order to do UI effects.

	//Use the below for animating meterFill?
	//float localFillAmount;

    public Image meterFill;


	void Start () 
	{
		meterFill.fillAmount = GetFillAmount();
	}
		
    public float GetFillAmount()
    {
		return GameManager.Instance.AbilityMeter;
    }

	//Called from GameManager.cs - Notifies us of value changes so we can do animations.
	public void OnMeterValueChanged( float fillAmount )
	{
		//TODO animations
		meterFill.fillAmount = fillAmount;

		//Detect if the ability meter was set to zero and assume an ability was used.. This may be bad if we ever have anything that takes away meter later.
		if( fillAmount == 0f )
			EmptyMeter();
	}

    //Called on ability use - MOVED TO GAMEMANGER.CS
    public void EmptyMeter()
    {
        //TODO animations
		meterFill.fillAmount = 0;
    }


}
