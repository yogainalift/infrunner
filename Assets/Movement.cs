using UnityEngine;
using System.Collections;
using System;

public class Movement : MonoBehaviour {

    [SerializeField]
    public float lerpSpeed;
    
	Rigidbody m_rigidbody;
    private readonly Vector3 _vel = new Vector3(0,0,50);

	Vector3 oldPosition;
	private Vector3 newPosition;

    void Awake() {
        newPosition = transform.position;
    }
    
    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
		newPosition = new Vector3(oldPosition.x +2.7f, oldPosition.y,oldPosition.z);
		oldPosition = m_rigidbody.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
//        m_rigidbody.velocity = _vel;
	

        PositionChanging();
	}

    void PositionChanging()
    {
        Vector3 pos = transform.position;
        Vector3 positionA = new Vector3( -2.7f, pos.y, pos.z );
        Vector3 positionB = new Vector3(0, pos.y, pos.z);
        Vector3 positionC = new Vector3( pos.x + 2.7f, pos.y, pos.z );

        if ( Input.GetKeyDown( KeyCode.Q ) && pos.x <= 0 ) {
            newPosition = positionA;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && pos.x > 0)
        {
            newPosition = positionB;
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            newPosition = positionB;
        }

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpSpeed);
    }
}
