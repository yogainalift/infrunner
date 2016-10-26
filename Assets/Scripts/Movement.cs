using UnityEngine;
using System.Linq;

#pragma warning disable 0414 

public class Movement : MonoBehaviour
{
   
  
    private float sideInput;


    //The speed of the left right movement
    public float LerpSpeed = 20f;

    //The x distance for left-right positions
    private static float AMMOUNT = 2.7f;

 

    //enum that holds possible positions
    public enum POS
    {
        LEFT,
        CENTER,
        RIGHT
    }
    ;

    Rigidbody myBody;

//    private bool isJumping;
//    private bool wasGrounded;
    private float lastGroundedTime;
    private float lastJumpTime;

    //initial position
    public POS myPos = POS.CENTER;

    public void SetPos(POS p)
    {
        myPos = p;
    }

    public POS GetPos()
    {
        return myPos;
    }

    //the new position if there is change
    private Vector3 newPosition;
    private float jumpPosition;


    void Awake()
    {
        newPosition = transform.position;
    }

    void Start()
    {
        myBody = this.GetComponent<Rigidbody>();

        sideInput = 0;
    }

    void Update()
    {
        //calling the right left movement procedure
        GetInput();
        PositionChanging();
    }

    void GetInput()
    {
        sideInput = Input.GetAxis("Horizontal");
    }


    void FixedUpdate()
    {
        //finaly perform the lerp and give player speed
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(newPosition.x, transform.position.y, transform.position.z),
            Time.deltaTime*LerpSpeed);
    }


    //holds the logic for the player movement
    void PositionChanging()
    {
        //pos variable for usability
        Vector3 pos = this.transform.position;

        //possible positions (this can probably be improved)
        Vector3 positionA = new Vector3(-AMMOUNT, pos.y, pos.z);
        Vector3 positionB = new Vector3(0.0f, pos.y, pos.z);
        Vector3 positionC = new Vector3(AMMOUNT, pos.y, pos.z);


        //detecting input and making changes accordingly
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown((KeyCode.RightArrow)))
        {
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
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown((KeyCode.LeftArrow)))
        {
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
        
    }

}