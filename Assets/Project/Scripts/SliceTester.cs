using UnityEngine;
using System.Collections;

public class SliceTester : MonoBehaviour 
{

	//public RectTransform testdummy;
	SpriteSlice slicer;
	void Start () {
		slicer = GetComponent<SpriteSlice>();
	}

	bool holding = false;
	Vector3 start = Vector3.zero;
	Vector3 lastPos = Vector3.zero;

	// Update is called once per frame
	void Update () 
	{
		if( Input.GetMouseButtonDown(0) )
		{
			holding = true;
			start = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			lastPos = start;
			return;
		}
		if( holding )
		{
			Vector3 end = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			end.z = 0f;
			//Debug.Log( Vector3.Angle( start, end ) );


			if( Input.GetMouseButton(0) == false )
			{
				/*
				start.z = 0f;
				Debug.DrawLine(start, end, Color.red, 2f);

				//bool isCorner = false;
				//slicer.SnapInbound( testdummy, start, out isCorner );
				//Debug.Log( isCorner );

				slicer.SliceSprite(start, end, testdummy);
				*/
				holding = false;
				lastPos = Vector3.zero;
			}
			else
			{
				//See if the mouse hit something since last frame
				RaycastHit2D data = Physics2D.Linecast( lastPos, end );
				if( data != null )
				{
					ISlicable s = data.collider.GetComponent<ISlicable>();
					if( s != null )
					{
						s.OnSliced();

						slicer.SliceSprite( lastPos, end, data.collider.GetComponent<RectTransform>() );
					}
				}

				lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			}


		}
	}
}
