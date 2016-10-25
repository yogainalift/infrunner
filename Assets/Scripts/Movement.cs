using UnityEngine;
using System.Linq;

public class Movement : MonoBehaviour
{
    //The Z(forward)-velocity of the player
    public float myVelocity;

    private float sideInput, jumpInput;

    public float jumpHeight = 4;
    public float downAccel = 1f;
    public float timeToJumpApex = .4f;
    float jumpVelocity;
    float gravity;
    private Vector3 vel;

    public LayerMask ground;

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

        gravity = -(2*jumpHeight)/Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity)*timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
        sideInput = jumpInput = 0;

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
        jumpInput = Input.GetAxisRaw("Jump");
    }


    void FixedUpdate()
    {
        //finaly perform the lerp and give player speed
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(newPosition.x, transform.position.y, transform.position.z + (myVelocity/LerpSpeed)),
            Time.deltaTime*LerpSpeed);
        Jump();
    }

    void Jump()
    {
        if ( IsGrounded() && jumpInput > 0) {
            //give y velocity
            myBody.velocity = new Vector3( 0, jumpHeight, 0 );
        }
        else if (IsGrounded() &&
                 jumpInput == 0)
        {
            //zero out y vel
            myBody.velocity = new Vector3(0,0,0);
        }
        else
        {
            //decrease y vel
            myBody.velocity -= new Vector3( 0, downAccel, 0 );
        }
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

    private bool IsGrounded()
    {

        var castDistance = .2f;

        var size = .3f;
        var halfHeight = .1f;

//        return Physics.Raycast(transform.position, Vector3.down, halfHeight, ground);

        var rays = new[]
        {
            new Vector3(0, halfHeight, 0),
            new Vector3(size, halfHeight, size),
            new Vector3(-size, halfHeight, -size),
            new Vector3(-size, halfHeight, size),
            new Vector3(size, halfHeight, -size),
            new Vector3(0, halfHeight, size),
            new Vector3(0, halfHeight, -size),
            new Vector3(-size, halfHeight, 0),
            new Vector3(size, halfHeight, 0),
            new Vector3(size/2, halfHeight, size/2),
            new Vector3(-size/2, halfHeight, -size/2),
            new Vector3(-size/2, halfHeight, size/2),
            new Vector3(size/2, halfHeight, -size/2),
            new Vector3(0, halfHeight, size/2),
            new Vector3(0, halfHeight, -size/2),
            new Vector3(-size/2, halfHeight, 0),
            new Vector3(size/2, halfHeight, 0),
            new Vector3(size/3, halfHeight, size/3),
            new Vector3(-size/3, halfHeight, -size/3),
            new Vector3(-size/3, halfHeight, size/3),
            new Vector3(size/3, halfHeight, -size/3),
            new Vector3(0, halfHeight, size/3),
            new Vector3(0, halfHeight, -size/3),
            new Vector3(-size/3, halfHeight, 0),
            new Vector3(size/3, halfHeight, 0),
        };

        foreach (var ray in rays)
        {
            Debug.DrawLine( transform.position - ray, transform.position - ray - (Vector3.down * castDistance) );
        }

        return rays.Any(ray => Physics.Raycast(transform.position - ray, Vector3.down, castDistance));
    }
}