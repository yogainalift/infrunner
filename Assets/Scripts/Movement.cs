using UnityEngine;
using System.Linq;

public class Movement : MonoBehaviour
{
    /**
     * Ground stuff 
     * 
     *
     */
    //The Z(forward)-velocity of the player
    public float myVelocity;

    public LayerMask ground;

    //The speed of the left right movement
    public float LerpSpeed = 20f;

    //The x distance for left-right positions
    private static float AMMOUNT = 2.7f;


    /**
             * Jump stuff 
             *
             * 
    */
    public float jumpHeight = 4;
    public float JumpSpeed = 5f;
    [Range(0.5f, 3)] public float FloatTime = 2f;
    public float downAccel = 1f;


    //enum that holds possible positions
    public enum POS
    {
        LEFT,
        CENTER,
        RIGHT
    }
    ;

    Rigidbody myBody;

    private bool isJumping;
//    private bool wasGrounded;
    private float jumpTime;
    private float timeOfJump;

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


        jumpTime = 0f;
    }

    void Update()
    {
        //calling the right left movement procedure
        PositionChanging();


        //if player performed jump action
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            //if not already jumping
            if (!isJumping)
            {
                isJumping = !isJumping;
                jumpTime = FloatTime;
            }
        }

        if (jumpTime > FloatTime/1.5f)
        {
            myBody.useGravity = false;
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, jumpHeight, transform.position.z), Time.deltaTime*JumpSpeed);

            jumpTime -= Time.deltaTime;
        }
        else if (jumpTime > FloatTime/5)
        {
            transform.position = Vector3.Lerp(transform.position,
                new Vector3(transform.position.x, 0.1f, transform.position.z), Time.deltaTime*downAccel);


            /**
             * IDEA
             * 
             * 
             * 
             * Receive jump help from pet underwater 
             * 
             * 
             * 
             * IDEA
             */
            isJumping = false;
        }
        if (transform.position.y <= 0.2f)
        {
            jumpTime = 0f;
            myBody.useGravity = true;
            
        }
    }


    void FixedUpdate()
    {
        //finaly perform the lerp and give player speed
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(newPosition.x, transform.position.y, transform.position.z + (myVelocity/LerpSpeed)),
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