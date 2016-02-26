using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteSlice : MonoBehaviour 
{
	public bool _doDebug = false;

	//GetWorldCorners order: (Lines up with enum)
	//0 - bottom left
	//1 - top left
	//2 - top right
	//3 - bottom right

	//Inner order:
	//0 - Bottom
	//1 - Left
	//2 - Top
	//3 - Right


	//



	public AnimationCurve innerFillMultiplier;
	public void SliceSprite( Vector3 entry, Vector3 exit, RectTransform obj )
	{
		// Take 2d ray. Create line. Origin -> exit point.
		//Snap origin to one of 8 edge points. Make sure that line contacts opposite sides of bounding box! 
		//
		// Radial 180 if inbound is not corner
		// Radial 90 if inbound is corner 
		
		// for new object, disable clockwise, invert fill amount

		// LineLineIntersection

		Debug.LogWarning("You should stop using this. It's cool but too complicated for our uses!", this);

		bool isStartCorner;
		int edgeIndex;
		Vector3 start = SnapToBounds( obj, entry, out isStartCorner, out edgeIndex );

		//Get exit
		bool doesntmatter; int doesntmatter2;
		Vector3 end = SnapToBounds( obj, exit, out doesntmatter, out doesntmatter2 );

		if( start == end )
			return;
		if( _doDebug )
			Debug.DrawLine(start, end, Color.white, 2f);


		Image original = obj.GetComponent<Image>(); //It is assumed that this will not be null!

		original.fillOrigin = edgeIndex;
		original.fillMethod = isStartCorner == true ? Image.FillMethod.Radial90 : Image.FillMethod.Radial180;

		if( isStartCorner )
		{
			Vector3 dir = VectorExtras.Direction(start, end);
			Vector3 rightAngle = edgeIndex % 2 == 0 ? obj.up : obj.right;
			float angle = Mathf.PingPong(Vector3.Angle( rightAngle, dir ), 90f);

			if( _doDebug )
				Debug.Log( angle );

			original.fillAmount = angle / 90f;
		}
		else
		{
			//Inner
			Vector3 rightAngle = edgeIndex % 2 == 0 ? obj.right : obj.up;
			Vector3 dir = (edgeIndex == 0 || edgeIndex == 3) ? -VectorExtras.Direction(start, end) : VectorExtras.Direction(start, end);
			float angle = Vector3.Angle( rightAngle, dir );

			if( _doDebug )
				Debug.Log( angle );

			original.fillAmount = (angle * innerFillMultiplier.Evaluate( angle/180f )) / 180f;
		}



		//Create the new object
		GameObject clone = GameObject.Instantiate( obj.gameObject );
		clone.transform.SetParent( obj.parent );

		Image cloneImg = clone.GetComponent<Image>();
		cloneImg.fillAmount = Mathf.PingPong( original.fillAmount + 1f, 1f );
		cloneImg.fillClockwise = !original.fillClockwise;

		Destroy( obj.GetComponent<Collider2D>() );
		Destroy( obj.gameObject, 10f );
		Destroy( clone.GetComponent<Collider2D>() );
		Destroy( clone, 10f );

		//TODO make clone inherit obj's speed and rotation

	}

	//Snap the inbound slice position to one of the 8 positions along the edge of our bounding box.
	Vector3 SnapToBounds( RectTransform obj, Vector3 pos, out bool isCorner, out int chosenIndex )
	{
		Vector3[] corners = new Vector3[4];
		Vector3[] inners = new Vector3[4];

		obj.GetWorldCorners( corners );
		corners = Flatten( corners );

		//Calculate inners based on corners
		inners[0] = Vector3.Lerp(corners[3], corners[0], 0.5f);
		inners[1] = Vector3.Lerp(corners[0], corners[1], 0.5f);
		inners[2] = Vector3.Lerp(corners[1], corners[2], 0.5f);
		inners[3] = Vector3.Lerp(corners[2], corners[3], 0.5f);

		//Debug.DrawRay( inners[3], Vector3.up, Color.cyan, 2f );

		//Find what is closest to pos
		isCorner = false;
		chosenIndex = -1;
		int index = -1;
		float distance = Mathf.Infinity; //TODO - instead of distance, base off of closest to original angle
		Vector3 closestPos = Vector3.zero;
		foreach( Vector3 c in corners )
		{
			index++;
			if( Vector3.Distance( c, pos ) < distance )
			{
				distance = Vector3.Distance( c, pos );
				closestPos = c;
				chosenIndex = index;
				isCorner = true;
			}
		}
		index = -1;
		foreach( Vector3 i in inners )
		{
			index++;
			if( Vector3.Distance( i, pos ) < distance )
			{
				distance = Vector3.Distance( i, pos );
				closestPos = i;
				chosenIndex = index;
				isCorner = false;
			}
		}

		if( _doDebug )
			Debug.DrawLine(pos, closestPos, Color.yellow, 2f);

		return closestPos;
	}





	Vector3[] Flatten( Vector3[] val )
	{
		for(int i = 0; i < val.Length; i++)
			val[i].z = 0f;
		return val;
	}
}
