using UnityEngine;
using System.Collections;

public class PointObject : MonoBehaviour, ISlicable 
{
	public int pointValue = 1;

	void ISlicable.OnSliced()
	{
		//Give us points
		GameManager.Instance.AddPoints( pointValue );
	}


	void OnCollisionEnter2D( Collision2D c )
	{
		if( c.collider.tag == "Destroy" )
			Destroy( this.gameObject );
	}

}
