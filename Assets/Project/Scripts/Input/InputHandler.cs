using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InputHandler : MonoBehaviour {

    public enum INPUT_MODE
    {
        NORMAL,
        ABILITY_TIME,
        ABILITY_SPACE
    }

    LineRenderer lr;
    SpriteSlice slicer;

    //List of positions stored because LineRenderer doesnt grant access to its vertecies
    List<Vector3> lineVerticies;

    //Standard Input control variables
    const float LINE_DRAW_ITERATOR = 0.005f;
    const float LINE_DELETE_ITERATOR = 0.05f;

    const float SLICE_TIME_LIMIT = 1.0f;

    private float timeDraw;
    private float timeDelete;

    private bool firstClick = true;

    //Laser control variables
    private float laserTimer;
    private bool laserCanFire;

    public GameObject laserPrefab;

    //Will be used to track over-all time for all three facets of input
    private float generalUseTimer;
    
    //Tracks current input mode
    private INPUT_MODE mode;

	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
        slicer = GetComponent<SpriteSlice>();

        lr.SetWidth(0.1f, 0.1f);

        lr.SetVertexCount(0);
        lineVerticies = new List<Vector3>();

        timeDelete = LINE_DELETE_ITERATOR;
        timeDraw = 0.0f;

        laserTimer = 0.0f;
        laserCanFire = true;

        generalUseTimer = 0.0f;

        mode = INPUT_MODE.NORMAL;
	}
	
	// Update is called once per frame
	void Update () 
	{
        switch(mode)
        {
            case INPUT_MODE.NORMAL:

                HandleNormalInput();

                break;

            case INPUT_MODE.ABILITY_TIME:

                HandleTimeInput();

                break;

            case INPUT_MODE.ABILITY_SPACE:

                HandleSpaceInput();

                break;

            default:
                break;
        }
	}

    void HandleNormalInput()
    {
        if (Input.GetAxis("Fire1") > 0)
        {
            if (generalUseTimer < SLICE_TIME_LIMIT)
            {
                generalUseTimer += Time.deltaTime;

                //This check prevents lifting mouse button, pressing it again and
                //having the new line continue from the old one
                if (firstClick)
                {
                    lineVerticies.Clear();
                    lr.SetVertexCount(0);
                    firstClick = false;
                }

                if (timeDraw < 0.0f)
                {
                    Vector3 v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    v.z = 0.0f;


					if( lineVerticies.Count > 0 )
					{
						Vector3 lastPosition = lineVerticies[lineVerticies.Count - 1];

						//See if the mouse hit something since last frame
						RaycastHit2D data = Physics2D.Linecast( lastPosition, v );
						if( data.collider != null )
						{
							ISlicable s = data.collider.GetComponent<ISlicable>();
							if( s != null )
							{

								//Tell the object that it has been hit with a slice.
								s.OnSliced();
								
							}
						}
					}


                    lineVerticies.Add(v);
                    lr.SetVertexCount(lineVerticies.Count);

                    //Draw all vertecies in their current position
                    for (int i = 0; i < lineVerticies.Count; i++)
                        lr.SetPosition(i, lineVerticies[i]);

                    timeDraw = LINE_DRAW_ITERATOR;
                }
                else
                {
                    timeDraw -= Time.deltaTime;
                }

                if (timeDelete <= 0.0f)
                {
                    //Remove first vertex to slowly delete the line
                    lineVerticies.RemoveAt(0);
                    lr.SetVertexCount(lineVerticies.Count);

                    timeDelete = LINE_DELETE_ITERATOR;
                }
                else
                {
                    timeDelete -= Time.deltaTime;
                }
            }
            else
            {
                HandleNormalMouseUp();
            }
        }
        else
        {
            HandleNormalMouseUp();
            generalUseTimer = 0.0f;
            firstClick = true;
        }
    }

    void HandleNormalMouseUp()
    {
        if (lineVerticies.Count > 0)
        {
            if (timeDelete <= 0.0f)
            {
                lineVerticies.RemoveAt(0);
                lr.SetVertexCount(lineVerticies.Count);

                //I had to put this loop in again or else the line got erased backwards.
                //Dunno why.
                for (int i = 0; i < lineVerticies.Count; i++)
                    lr.SetPosition(i, lineVerticies[i]);

                timeDelete = 0.025f;
            }
            else
            {
                timeDelete -= Time.deltaTime;
            }
        }
    }

    void HandleSpaceInput()
    {
        //If it hasn't been 4 seconds yet
        if (laserTimer < 4.0f)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(laserCanFire)
                {
                    GameObject g = GameObject.Instantiate(laserPrefab);
                }
                //create lasers here
                Debug.Log("New laser!");
            }
            else
            {
                //Reset the input gate. Player MUST lift finger to spawn new laser
                laserCanFire = true;
            }
            laserTimer += Time.deltaTime;
        }
        //Return to normal input once time is up
        else
        {
            mode = INPUT_MODE.NORMAL;
            laserTimer = 0;
        }
    }

    void HandleTimeInput()
    {
        //Test junk
        Debug.Log("TIME MAGIC YO!");
        SetInputMode(INPUT_MODE.NORMAL);
    }

    public void SetInputMode(INPUT_MODE newMode_)
    {
        mode = newMode_;
        generalUseTimer = 0.0f;
    }

    public INPUT_MODE GetInputMode()
    {
        return mode;
    }
}
