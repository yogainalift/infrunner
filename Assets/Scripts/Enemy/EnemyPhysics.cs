using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

#pragma warning disable 0414 

public class EnemyPhysics : Entity
{
    const float hitTime = 0.05f;
    public GameObject lookTarget;
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .025f;
    float accelerationTimeGrounded = .05f;
    public float patrolSpeed = 8;

    [Range(0, 15)] public float chaseDistance;
    float playerDistance;
    public float rotationDamping;
    public float patrolWaitTime;
    public Vector3[] patrolWaypoints;
    Vector3[] globalWaypoints;

    private float patrolTimer; // A timer for the patrolWaitTime.
    private int wayPointIndex = 0; // A counter for the way point array.

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;

    EnemyController controller;
    public GameObject explosion;

    private Image healthBar;
    float maxHealth;

    void Start()
    {
        controller = GetComponent<EnemyController>();

        gravity = -(2*jumpHeight)/Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity)*timeToJumpApex;

        globalWaypoints = new Vector3[patrolWaypoints.Length];
        for (int i = 0; i < patrolWaypoints.Length; i++)
        {
            globalWaypoints[i] = patrolWaypoints[i] + transform.position;
        }

        healthBar = transform.FindChild("EnemyCanvas").FindChild("HealthBG").FindChild("Health").GetComponent<Image>();
        maxHealth = health; //max health is the health when initialized
    }


    void Update()
    {
        if (lookTarget == null)
        {
            lookTarget = GameObject.Find("Player") as GameObject;
        }
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        if (health <= 0)
        {
            Instantiate(explosion, transform.position, new Quaternion());
            Destroy(this.gameObject);
        }

        if (lookTarget != null)
        {
            playerDistance = Mathf.Abs(lookTarget.transform.position.x - transform.position.x);
        }

        if (playerDistance > chaseDistance)
        {
            patrol();
        }
        if (playerDistance < chaseDistance)
        {
            chase();
            if (playerDistance < 4f)
            {
                attack();
            }
        }


        velocity.y += gravity*Time.deltaTime;

        controller.Move(velocity*Time.deltaTime);
    }

    void lookAtPlayer()
    {
        Vector3 targetPosition = lookTarget.transform.position;

        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);
        /*
		Quaternion rotation = Quaternion.LookRotation (lookTarget.transform.position - transform.position);
		// rotate an object from a point to another in a given amount of time
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime * rotationDamping);
		*/
    }

    void chase()
    {
        float dirX = (lookTarget.transform.position.x - transform.position.x > 0) ? 1 : -1;

        float targetVelocityX = dirX*patrolSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
            (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
    }

    void attack()
    {
    }

    void patrol()
    {
        if (Mathf.Abs(globalWaypoints[wayPointIndex].x - transform.position.x) > 0.1)
        {
            float dirX = (globalWaypoints[wayPointIndex].x - transform.position.x > 0) ? 1 : -1;


            float targetVelocityX = dirX*patrolSpeed;
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing,
                (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);

            // Reset the timer.
        }
        else
        {
            patrolTimer += Time.deltaTime;

            if (patrolTimer >= patrolWaitTime)
            {
                patrolTimer = 0;
                if (wayPointIndex == globalWaypoints.Length - 1)
                {
                    wayPointIndex = 0;
                }
                else
                {
                    wayPointIndex++;
                }
            }
            velocity.x = 0;
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


    void OnDrawGizmos()
    {
        if (patrolWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = .3f;

            for (int i = 0; i < patrolWaypoints.Length; i++)
            {
                Vector3 globalWaypointPos = (Application.isPlaying)
                    ? globalWaypoints[i]
                    : patrolWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPos - Vector3.up*size, globalWaypointPos + Vector3.up*size);
                Gizmos.DrawLine(globalWaypointPos - Vector3.left*size, globalWaypointPos + Vector3.left*size);
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Arrow")
        {
            StartCoroutine(flashHit(hitTime, this.gameObject));
            gameObject.GetComponent<Entity>().TakeDamage(10); //give damage ammount

            float newSpeed; //to calculate pushBack
            chaseDistance = 100; //change chase distance to something big so that the enemy will always chase
            //            newSpeed = collision.relativeVelocity.magnitude * collision.gameObject.GetComponent<ArrowControl>().pushBackDistance; //add pushBack velocity depending on the direction of the arrow
            newSpeed = 2f;
            SetVelocity(new Vector3(newSpeed*Mathf.Sign(collision.relativeVelocity.x), velocity.y, velocity.z));

            this.healthBar.fillAmount = (float) this.health/(float) this.maxHealth;
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            Entity playerEntity = c.gameObject.GetComponent<Entity>();
//            Player myPlayer = c.gameObject.GetComponent<Player>();
            if (playerEntity.health <= 10)
            {
                this.chaseDistance = 0;
            }
            playerEntity.TakeDamage(10, this.transform); //give damage ammount
//            myPlayer.healthBar.fillAmount = (float)myPlayer.health / (float)myPlayer.maxHealth;
//            myPlayer.healthText.text = (myPlayer.healthBar.fillAmount*100).ToString();
            StartCoroutine(flashHit(hitTime, c.gameObject));
        }
    }

    IEnumerator flashHit(float waitTime, GameObject obj)
    {
        Renderer renderer;

        if (obj.name == "ShiaBoss")
        {
            renderer = obj.GetComponentInChildren<Renderer>();
        }
        else
        {
            renderer = obj.GetComponent<Renderer>();
        }
        Color originalColor = renderer.material.color;
        renderer.material.color = Color.red;
        yield return new WaitForSeconds(waitTime);
        renderer.material.color = originalColor;
    }
}