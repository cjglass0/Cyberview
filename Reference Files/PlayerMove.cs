using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    /////////////////////////////////////////////////////////////////////////////////////////// FIELDS ///////////////////////////////////////////////////////////////////
    public PlayerCtrl controller;
    public GameObject LeftIndicator;
    public GameObject RightIndicator;
    public GameObject TopIndicator;

    //Settings (for floats, check in Start)
    private float runSpeed;
    private bool debugInputs = false; //<------- Debug

    //Player Input Properties
    float horizontalMove = 0f;
    bool jump = false;
    bool crouch = false;

    //Input processing
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    //Visualizing
    private static readonly int NONE = 9;
    private static readonly int LEFT = 0;
    private static readonly int RIGHT = 1;
    private static readonly int TOP = 2;
    private static readonly int BEGIN = 3;
    private static readonly int STOP = 4;
    private bool [] input1Showing = { false, false, false };
    private bool [] input2Showing = { false, false, false };
    private bool [] input3Showing = { false, false, false };

    ////////////////////////////////////////////////////////////////////////////////////////// START () ///////////////////////////////////////////////////////////////////
    void Start()
    {
        //Settings:
        runSpeed = 1000;
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen
        LeftIndicator.SetActive(false);
        RightIndicator.SetActive(false);
        TopIndicator.SetActive(false);
    }

    ////////////////////////////////////////////////////////////////////////////////////////// UPDATE () ///////////////////////////////////////////////////////////////////
    void Update()
    {
        ProcessInputs();
    }

    ///////////////////////////////////////////////////////////////////////////////////// FIXED UPDATE () //////////////////////////////////////////////////////////////////
    void FixedUpdate()
    {
        // Move our character
        controller.Move(horizontalMove * Time.fixedDeltaTime, jump);
        jump = false;
    }

    ////////////////////////////////////////////////////////////////////////////////////// CUSTOM METHODS /////////////////////////////////////////////////////////////////
    ///
    /////////////////////////////////////////////////////////////////////////////////////////// ProcessInputs () ####################
    private void ProcessInputs()
    {
        int curState = NONE;

        if (Input.touchCount > 0) // user is touching the screen 
        {
            for (int i = 0; i < Input.touchCount; i++) // iterate processing for each touch
            {
                Touch touch = Input.GetTouch(i); // get the touch
                if (touch.position.y > (Screen.height / 10) * 6) //##### Top Touch
                {
                    if (debugInputs) Debug.Log("top touch");
                    //top
                    jump = true;
                    curState = TOP;
                }
                else
                {

                    if (touch.position.x < Screen.width / 2) //##### Left Touch
                    {
                        if (debugInputs) Debug.Log("left touch");
                        //left
                        horizontalMove = -runSpeed;
                        curState = LEFT;
                    }
                    else
                    {
                        if (debugInputs) Debug.Log("right touch");          //##### Right Touch
                                                                            //right
                        horizontalMove = runSpeed;
                        curState = RIGHT;
                    }

                }
                // Save Visualized Input
                VisualizeInputs(BEGIN, curState);
                //switch input if a finger has moved from one area to another
                switch (i)
                {
                    case 2:
                        input3Showing[curState] = true;
                        for (int j = 0; j < input3Showing.Length; j++) {
                            if (input3Showing[j] == true && j != curState){  VisualizeInputs(STOP, j); input3Showing[j] = false; if (debugInputs) Debug.Log("input "+i+" moved area"); }}
                        break;
                    case 1:
                        input2Showing[curState] = true;
                        for (int j = 0; j < input2Showing.Length; j++) {
                            if (input2Showing[j] == true && j != curState) { VisualizeInputs(STOP, j); input2Showing[j] = false; if (debugInputs) Debug.Log("input " + i + " moved area"); }}
                        break;
                    default:
                        input1Showing[curState] = true;
                        for (int j = 0; j < input1Showing.Length; j++) {
                            if (input1Showing[j] == true && j != curState) { VisualizeInputs(STOP, j); input1Showing[j] = false; if (debugInputs) Debug.Log("input " + i + " moved area"); } }
                        break;
                }


                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    VisualizeInputs(STOP, curState);
                    lp = touch.position;  //last touch position. Ommitted if you use list 

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
                            {   //Right swipe
                                if (debugInputs) Debug.Log("Right Swipe");
                            }
                            else
                            {   //Left swipe
                                if (debugInputs) Debug.Log("Left Swipe");
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lp.y > fp.y)  //If the movement was up
                            {   //Up swipe
                                if (debugInputs) Debug.Log("Up Swipe");
                            }
                            else
                            {   //Down swipe
                                if (debugInputs) Debug.Log("Down Swipe");
                            }
                        }
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        if (debugInputs) Debug.Log("Tap");
                    }
                }
            }
        }
        else
        {
            horizontalMove = 0;
            //disable visual feedback when no touches
            for (int i=0; i<3; i++)
            {
                input1Showing[i] = false;
                input2Showing[i] = false;
                input3Showing[i] = false;
            }
        }
    }

    /////////////////////////////////////////////////////////////////////////////////////////// VisualizeInputs () ####################
    private void VisualizeInputs(int beginOrEnd, int state)
    {
        GameObject indicator = null;
        if (state == LEFT) indicator = LeftIndicator;
        else if (state == RIGHT) indicator = RightIndicator;
        else if (state == TOP) indicator = TopIndicator;

        if (beginOrEnd == BEGIN && !indicator.activeInHierarchy)
        {
            indicator.SetActive(true);
        } else if (beginOrEnd == STOP && indicator.activeInHierarchy)
        {
            indicator.SetActive(false);
        }
    }

}