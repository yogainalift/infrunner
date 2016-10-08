using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {


    //The Z(forward)-velocity of the player
    public float myVelocity;

    //The speed of the left right movement
    public float lerpSpeed;

    //The x distance for left-right positions
    private static float AMMOUNT = 2.7f;

    //enum that holds possible positions
    public enum POS
        {
            LEFT,
            CENTER,
            RIGHT
        };


    //initial position
    public POS myPos = POS.CENTER;

    //the new position if there is change
	private Vector3 newPosition;

    void Awake() {
        newPosition = transform.position;
    }
    
    void Start () {
    }
	
	void Update () {
        //calling the right left movement procedure
        PositionChanging();
    }

    //holds the logic for the player movement
    void PositionChanging()
    {

        //pos variable for usability
        Vector3 pos = transform.position;

        //possible positions (this can probably be improved)
        Vector3 positionA = new Vector3( -AMMOUNT, pos.y, pos.z);
        Vector3 positionB = new Vector3(0.0f, pos.y, pos.z);
        Vector3 positionC = new Vector3( AMMOUNT, pos.y, pos.z);


        //detecting input and making changes accordingly
        if ( Input.GetKeyDown( KeyCode.E ) ) {
            if (myPos == POS.LEFT)
            {
                myPos = POS.CENTER;
                newPosition = positionB;
            }
            else
            {
                myPos = POS.RIGHT;
                newPosition = positionC;
            }
        }
        else if ( Input.GetKeyDown( KeyCode.Q ) ) {
            if (myPos == POS.RIGHT)
            {
                myPos = POS.CENTER;
                newPosition = positionB;
            }
            else
            {
                myPos = POS.LEFT;
                newPosition = positionA;
            }
        }

        //finaly perform the lerp and give player speed
        transform.position = Vector3.Lerp(transform.position, new Vector3(newPosition.x, pos.y, transform.position.z + (myVelocity / lerpSpeed) ), Time.deltaTime * lerpSpeed);
    }
}
