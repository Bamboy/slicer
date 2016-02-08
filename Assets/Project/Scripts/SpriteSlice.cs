using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpriteSlice : MonoBehaviour 
{
	public bool _doDebug = false;





	//Snap the inbound slice position to one of the 8 positions along the edge of our bounding box.
	public Vector3 SnapToBounds( RectTransform obj, Vector3 pos, out bool isCorner )
	{
		Vector3[] corners = new Vector3[4];
		Vector3[] inners = new Vector3[4];

		obj.GetWorldCorners( corners );
		corners = Flatten( corners );

		//Calculate inners based on corners
		inners[0] = Vector3.Lerp(corners[0], corners[1], 0.5f);
		inners[1] = Vector3.Lerp(corners[1], corners[2], 0.5f);
		inners[2] = Vector3.Lerp(corners[2], corners[3], 0.5f);
		inners[3] = Vector3.Lerp(corners[3], corners[0], 0.5f);

		//Find what is closest to pos
		isCorner = false;
		float distance = Mathf.Infinity; //TODO - instead of distance, base off of closest to original angle
		Vector3 closestPos = Vector3.zero;
		foreach( Vector3 c in corners )
		{
			if( Vector3.Distance( c, pos ) < distance )
			{
				distance = Vector3.Distance( c, pos );
				closestPos = c;
				isCorner = true;
			}
		}
		foreach( Vector3 i in inners )
		{
			if( Vector3.Distance( i, pos ) < distance )
			{
				distance = Vector3.Distance( i, pos );
				closestPos = i;
				isCorner = false;
			}
		}

		if( _doDebug )
			Debug.DrawLine(pos, closestPos, Color.yellow, 2f);
		
		return closestPos;
	}

	public void SliceSprite( Vector3 entry, Vector3 exit, RectTransform obj )
	{
		// Take 2d ray. Create line. Origin -> exit point.
		//Snap origin to one of 8 edge points. Make sure that line contacts opposite sides of bounding box! 
		//
		// Radial 180 if inbound is not corner
		// Radial 90 if inbound is corner 
		
		// for new object, disable clockwise, invert fill amount

		// LineLineIntersection

		bool isStartCorner;
		Vector3 start = SnapToBounds( obj, entry, out isStartCorner );

		//Get exit
		bool doesntmatter;
		Vector3 end = SnapToBounds( obj, exit, out doesntmatter );

		if( start == end )
			return;
		if( _doDebug )
			Debug.DrawLine(start, end, Color.white, 2f);


		Image original = obj.GetComponent<Image>(); //It is assumed that this will not be null!

		original.fillMethod = isStartCorner == true ? Image.FillMethod.Radial90 : Image.FillMethod.Radial180;


		float angle = Vector3.Angle( start, end );

		original.fillAmount = angle/180f;





	}


	//Not being used!
	Vector3 FindExitIntersection( RectTransform obj, Vector3 entry, Vector3 exit )
	{
		Vector3[] corners = new Vector3[4];
		obj.GetWorldCorners( corners );
		corners = Flatten( corners );

		Vector3 intersectionPos;
		bool intersected = false;

		intersected = Math3d.LineLineIntersection( out intersectionPos, entry, exit, corners[0], corners[1] );
		if( intersected ) return intersectionPos;
		intersected = Math3d.LineLineIntersection( out intersectionPos, entry, exit, corners[1], corners[2] );
		if( intersected ) return intersectionPos;
		intersected = Math3d.LineLineIntersection( out intersectionPos, entry, exit, corners[2], corners[3] );
		if( intersected ) return intersectionPos;
		intersected = Math3d.LineLineIntersection( out intersectionPos, entry, exit, corners[3], corners[0] );
		if( intersected ) return intersectionPos;

		Debug.LogError("Something went horribly wrong!", this);
		return Vector3.zero;
	}






	Vector3[] Flatten( Vector3[] val )
	{
		for(int i = 0; i < val.Length; i++)
			val[i].z = 0f;
		return val;
	}
}
