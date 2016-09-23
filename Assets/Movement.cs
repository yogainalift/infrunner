using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Rigidbody m_rigidbody;
    Vector3 vel = new Vector3(0,0,50);

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();

    }
	
	// Update is called once per frame
	void Update () {
        m_rigidbody.velocity = vel;
    }
}
