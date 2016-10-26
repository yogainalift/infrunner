using UnityEngine;
using System.Collections;

public class enemyAIScript : MonoBehaviour {
	
	public float playerDistance;
	public float rotationDamping;
	public float moveSpeed;

	// Use this for initialization
	void Start () {
		rotationDamping = 5;
		moveSpeed = 1.5f;
	}
	
	// Update is called once per frame
	void Update () {
		// Calculate distance between player and enemy
		playerDistance = Vector2.Distance (GameObject.Find("Player").transform.position, transform.position);
		if (playerDistance < 15f)
		{
			lookAtPlayer();
		}
		if (playerDistance < 12f)
		{
			if(playerDistance > 4f)
			{
				chase ();
			}
			else if(playerDistance < 4f)
			{
				attack ();
			}
		}


	}

	void lookAtPlayer()
	{
		Quaternion rotation = Quaternion.LookRotation (GameObject.Find("Player").transform.position - transform.position);
		// rotate an object from a point to another in a given amount of time
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
	}

	void chase() 
	{
		// the enemy will move forward (enemy.forward) at speed moveSpeed * DeltaTime
		transform.Translate (Vector3.forward * moveSpeed * Time.deltaTime);

	}

	void attack()
	{
		RaycastHit hit;
		if (Physics.Raycast (transform.position, transform.forward, out hit))
		{
			if(hit.collider.gameObject.tag == "Player")
			{

			}
		}

	}
}
