using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointObject : MonoBehaviour, ISlicable
{
    public int pointValue = 1;

    //Make this value between 0 and 1
    public float meterFill;


    public FullSwingMeterScript fsmc;

    void Start()
    {
        //We can happily find a more efficient way to do this. Dragging and dropping wasnt working for me
        fsmc = GameObject.Find("Swing Bar").GetComponent<FullSwingMeterScript>();
    }

    void ISlicable.OnSliced()
    {
        //Give us points
        GameManager.Instance.AddPoints(pointValue);

        //Temporary amount. Can be changed obviously
        fsmc.AddToMeter(0.007f);

        //Trigger "Slice" event in wwise
        AkSoundEngine.PostEvent("Slice", GameObject.Find("UI Canvas"));

        DestroyImmediate(gameObject);
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.tag == "Destroy")
            Destroy(this.gameObject);
    }
}
