using UnityEngine;


public class Player : Entity
{
    //The Z(forward)-velocity of the player
    public float surfSpeed = 12;

    //The speed of the left right movement
    public float LerpSpeed = 20f;


    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .05f;
    float accelerationTimeGrounded = .05f;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    bool doubleJump = false;

    Controller controller;

    public AudioSource landingSound; //NOT IMPLEMENTED YET
    //if (velocity.y < -15){
    //    landingSound.mute = false
    //}


//    public Image healthBar;
//    public Text healthText;
//    public float maxHealth;


    private float sideInput;
    //The x distance for left-right positions
    private static float AMMOUNT = 2.7f;

    //enum that holds possible positions
    private enum POS
    {
        LEFT,
        CENTER,
        RIGHT
    };

    //initial position
    private POS myPos = POS.CENTER;

    //the new position if there is change
    private Vector3 newPosition;

    void Awake()
    {
        newPosition = transform.position;
    }

    void Start()
    {
        controller = GetComponent<Controller>();

        gravity = -(2*jumpHeight)/Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity)*timeToJumpApex;

        //this looks so bad but dunno a better way than this.
//        healthBar =GameObject.Find("HealthbarSystem").GetComponent<Transform>().transform.FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
//        healthText = GameObject.Find("HealthbarSystem").GetComponent<Transform>().transform.FindChild("HealthBG").FindChild("Health").FindChild("Text").GetComponent<Text>();
//        maxHealth = health; //max health is the health when initialized
//        healthBar.fillAmount = 1;
//        healthText.text = "100";
    }

    void Update()
    {
        if (controller.collisions.above)
        {
            velocity.y = 0;
        }
        if (controller.collisions.below)
        {
            velocity.y = 0;
            doubleJump = false;
        }

        //single jump
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        //double jump
        if (!controller.collisions.above && !controller.collisions.below
            && !doubleJump
            && (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)))
        {
            velocity.y = jumpVelocity;
            doubleJump = true;
        }


        float targetVelocityX = surfSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity*Time.deltaTime;
        controller.Move(velocity*Time.deltaTime);
        PositionChanging();
    }

    void FixedUpdate()
    {
        //finaly perform the lerp and give player speed
        transform.position = Vector3.Lerp(transform.position,
            new Vector3(newPosition.x, transform.position.y, transform.position.z),
            Time.deltaTime*LerpSpeed);
    }

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

    public void SetVelocity(Vector3 vel)
    {
        velocity += vel;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }
}