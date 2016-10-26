using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Entity : MonoBehaviour {

    private Color[] colors = { Color.white, Color.black };
    public float health;
	public GameObject ragdoll;
    bool invincible = false;
	//TEMPORARY //////////////////////////////////////////////////////////
	public GameObject player;
    //TEMPORARY //////////////////////////////////////////////////////////

    IEnumerator flashHit(float waitTime)
    {
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        renderer.material.color = Color.white;
        yield return new WaitForSeconds(waitTime);
        renderer.material.color = originalColor;
    }

    public void TakeDamage(float dmg)
    {
        if (this.gameObject.name == "Player")
        {
            if (!invincible)
            {
                health -= dmg;
                StartCoroutine(FlashPlayer(0.3f, 0.07f));
            }
        }
        else
        {
            health -= dmg;
        }
        checkIfDead();

        
    }

    public void TakeDamage(float dmg, Transform damageGiver)
    {
        if (this.gameObject.name == "Player")
        {
            if (!invincible)
            {
                health -= dmg;
                StartCoroutine(FlashPlayer(0.3f,0.07f));
                float newSpeed;  //to calculate pushBack (maybe this will change in the future)
                newSpeed = 35; //push back player
                Vector3 velocity = GetComponent<Player>().GetVelocity();

                this.gameObject.GetComponent<Player>().SetVelocity(new Vector3(
                    newSpeed * Mathf.Sign(this.transform.position.x - damageGiver.transform.position.x), velocity.y, velocity.z));
                //I WANT TO PUT THIS IN ENTITY BUT I CANNOT FIGURE OUT HOW TO GET THE COLLIDED OBJECTS INFO WITHOUT PASSING ITS VALUE
            }
        }
        else
        {
            health -= dmg;
        }
        checkIfDead();

        
    }

    void checkIfDead()
    {
        if (health <= 0)
        {
            Die();
        }
    }

	public void Die(){
		//Debug.Log ("DIE");

		if (this.tag=="Player"){
			Ragdoll r =	(Instantiate(ragdoll, transform.position, transform.rotation) as GameObject).GetComponent<Ragdoll>();
			r.CopyPose(transform);
			DestroyObject(r.gameObject,0f); //destroy ragdoll
            Destroy(this.gameObject, 0.5f); //destroy player
            StartCoroutine(waitFor(0.5f));  //respawn after waitFor
            
        }
	}

    IEnumerator FlashPlayer(float time, float intervalTime)
    {
        gameObject.GetComponent<BoxCollider>().isTrigger = false;
        Renderer renderer = GetComponent<Renderer>();
        Color originalColor = renderer.material.color;
        invincible = true;
        float elapsedTime = 0f;
        int index = 0;

        while (elapsedTime < time)
        {
            renderer.material.color = colors[index % 2];
            elapsedTime += Time.deltaTime;
            index++;
            yield return new WaitForSeconds(intervalTime);
        }
        invincible = false;
        renderer.material.color = originalColor;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }



    IEnumerator waitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        //if (!GameObject.Find("Player"))
        //    this.GetComponent<GameManager>().SpawnPlayer();
        GameObject spawnPoint = GameObject.Find("SpawnPoint");
        GameObject playerInstance = Instantiate(player, spawnPoint.transform.position, Quaternion.identity) as GameObject;
        playerInstance.name = "Player";

    }
}
