using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PointObject : MonoBehaviour, ISlicable
{
    //For the Explode script
   // public GameObject Explodepop;
    public int pointValue = 1;

    //Make this value between 0 and 1
    public float meterValue = 0.007f;

    void ISlicable.OnSliced()
    {
        //Give us points
        GameManager.Instance.AddPoints( pointValue );

        //Temporary amount. Can be changed obviously
		GameManager.Instance.AddMeter( meterValue );


		CameraShake.Instance.ShakeCamera( 0.175f + (0.075f * pointValue), 0.15f * pointValue );

        //place sound here

        Destroy(gameObject);
       // Instantiate(Explodepop, transform.position, new Quaternion()); //I've put my code and it should work with the Explode Effect Script ~ Leland
    }


    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.collider.tag == "Destroy")
		{
			if( GameManager.GameMode == GameModes.Pitch )
			{
				GameManager.Instance.Lives -= 1;
			}


			Destroy(this.gameObject);
		}
    }
}
