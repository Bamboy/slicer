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

    int vertexIndex;
    int vertexDeletionIndex;

    //List of positions stored because LineRenderer doesnt grant access to its vertecies
    List<Vector3> lineVerticies;

    //Timer variables
    const float LINE_DRAW_ITERATOR = 0.005f;
    const float LINE_DELETE_ITERATOR = 0.05f;

    private float timeDraw;
    private float timeDelete;

    private float laserTimer;

    private bool firstClick = true;

    private INPUT_MODE mode;

	// Use this for initialization
	void Start () {
        lr = GetComponent<LineRenderer>();
        slicer = GetComponent<SpriteSlice>();

        lr.SetWidth(0.1f, 0.1f);

        lr.SetVertexCount(0);
        lineVerticies = new List<Vector3>();

        vertexDeletionIndex = 0;

        timeDelete = LINE_DELETE_ITERATOR;
        timeDraw = 0.0f;

        laserTimer = 0.0f;

        mode = INPUT_MODE.NORMAL;
	}
	
	// Update is called once per frame
	void Update () {

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
            firstClick = true;

            if (lineVerticies.Count > 0)
            {
                if (timeDelete <= 0.0f)
                {
                    if (vertexDeletionIndex < lineVerticies.Count)
                    {
                        lineVerticies.RemoveAt(0);
                        lr.SetVertexCount(lineVerticies.Count);

                        //I had to put this loop in again or else the line got erased backwards.
                        //Dunno why.
                        for (int i = 0; i < lineVerticies.Count; i++)
                            lr.SetPosition(i, lineVerticies[i]);

                        timeDelete = 0.025f;
                    }
                }
                else
                {
                    timeDelete -= Time.deltaTime;
                }
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
                //create lasers here
                Debug.Log("New laser!");
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
    }
}
