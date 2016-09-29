using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

	[SerializeField]
	private float lerpSpeed;
    
	Rigidbody m_rigidbody;
    Vector3 vel = new Vector3(0,0,50);

	Vector3 oldPos;
	Vector3 newPos;

    // Use this for initialization
    void Start () {
        m_rigidbody = GetComponent<Rigidbody>();
		newPos = new Vector3(oldPos.x +2.7f, oldPos.y,oldPos.z);
		oldPos = m_rigidbody.transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        m_rigidbody.velocity = vel;
	
		if (Input.GetKeyDown (KeyCode.RightArrow)) {
			m_rigidbody.MovePosition(new Vector3(this.m_rigidbody.position.x + 2.8f, this.m_rigidbody.position.y, this.m_rigidbody.position.z)); 
			//m_rigidbody.transform.position = Vector3.Lerp(oldPos, newPos, Time.deltaTime * 3);
		}
		else if(Input.GetKeyDown (KeyCode.LeftArrow)) {
			m_rigidbody.MovePosition(new Vector3(this.m_rigidbody.position.x - 2.8f, this.m_rigidbody.position.y, this.m_rigidbody.position.z)); 
			//Vector3 goLeft = new Vector3(m_rigidbody.position.x - 2.7f, m_rigidbody.position.y, m_rigidbody.position.z);
		}
	}
}
