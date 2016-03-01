using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LaserScript : MonoBehaviour {

    public LineRenderer lr;

    private List<Vector3> points;

    //Laser control variables
    private const float LASER_SPEED = 1.0f;
    private const float LASER_LIFETIME = 2.0f;

    private int laserAtIndex;
    private int LastLaserIndex
    {
        get 
        {
            if (laserAtIndex + 1 >= numPoints)
                return 0;
            else
                return laserAtIndex + 1;
        }
    }

    private float laserTimer;

    //Total number of points to be drawn by the line renderer
    private const int numPoints = 12;

    private Vector3 laserDirection;

	// Use this for initialization
	void Start () {

        lr = GetComponent<LineRenderer>();
        lr.SetWidth(0.1f, 0.1f);
        lr.SetVertexCount(numPoints);

        laserTimer = 0.0f;
        laserAtIndex = numPoints - 1;
        
        points = new List<Vector3>();

        //Add all points at the same spot
        for(int i = 0; i < numPoints; i++)
        {
            Vector3 m_ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            m_.z = 0.0f;
            points.Add(m_);
        }

        //Generate random direction and adjust to set speed
        laserDirection = new Vector3(Mathf.Cos(Random.Range(-1.0f, 1.0f)), Mathf.Sin(Random.Range(-1.0f, 1.0f)), 0);
        laserDirection = laserDirection.normalized * LASER_SPEED;

	}
	
	// Update is called once per frame
    void Update()
    {
        if (laserTimer < LASER_LIFETIME)
        {
            //Move point at index ahead of the current leading point
            points[laserAtIndex] = points[LastLaserIndex] + laserDirection;

            /* Laser Collision Detection */
            {
                Vector3 lastPosition = points[LastLaserIndex];

                //See if the mouse hit something since last frame
                RaycastHit2D data = Physics2D.Linecast(lastPosition, points[laserAtIndex]);
                if (data.collider != null)
                {
                    ISlicable s = data.collider.GetComponent<ISlicable>();
                    if (s != null)
                    {

                        //Tell the object that it has been hit with a slice.
                        s.OnSliced();

                    }
                }
            }

            //project that point into screen space to test its bounds
            Vector3 screenSpace = Camera.main.WorldToScreenPoint(points[laserAtIndex]);

            if (screenSpace.x < 0 || screenSpace.x > Screen.width)
            {
                laserDirection.x *= -1;
            }
            if (screenSpace.y < 0 || screenSpace.y > Screen.height)
            {
                laserDirection.y *= -1;
            }

            //The following loop ensures that a single line is drawn and that
            //theres is no looping of points. Rearranges draw order without touching
            //actual order of the points
            int i = laserAtIndex;
            int x = 0;

            while (true)
            {
                lr.SetPosition(x, points[i]);

                x++;
                i++;

                if (i >= numPoints)
                    i = 0;

                if (i == laserAtIndex)
                    break;
            }


            //set new laser index
            if (laserAtIndex - 1 >= 0)
                laserAtIndex -= 1;
            else
                laserAtIndex = numPoints - 1;

            laserTimer += Time.deltaTime;
        }
        else
            Destroy(gameObject);
    }
}
