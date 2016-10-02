using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {

    [SerializeField]
    public float lerpSpeed;


        enum POS
        {
            POSA,
            POSB,
            POSC
        };

    
	Rigidbody m_rigidbody;
    private readonly Vector3 _vel = new Vector3(0,0,50);

	private Vector3 newPosition;

    void Awake() {
        newPosition = transform.position;
    }
    
    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        //m_rigidbody.velocity = _vel;
	    

        PositionChanging();
	}

    void PositionChanging()
    {
        Vector3 pos = transform.position;
        Vector3 positionA = new Vector3( -2.7f, pos.y, pos.z );
        Vector3 positionB = new Vector3(0.01f, pos.y, pos.z);
        Vector3 positionC = new Vector3( 2.7f, pos.y, pos.z );

        if ( Input.GetKeyDown( KeyCode.Q ) && pos.x <= 0.02f )
        {
            newPosition = positionA;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && pos.x > 0.00f )
        {
            newPosition = positionB;
        }
        else if ( Input.GetKeyDown( KeyCode.E ) && pos.x >= 0.00f ) {
            newPosition = positionC;
        }
        else if (Input.GetKeyDown(KeyCode.E) && pos.x < 0.00f )
        {
            newPosition = positionB;
        }


        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);
    }
}
