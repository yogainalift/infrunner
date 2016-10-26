using UnityEngine;
using System.Collections;

public class FallingDeath : MonoBehaviour {

	//GameManager gm = new GameManager();
	public Transform spawnPoint;
	public GameObject player;

	void OnTriggerEnter(Collider c) {
		if (c.tag == "Player"){
            c.gameObject.GetComponent<Entity>().TakeDamage(10000);
            /*Destroy (GameObject.FindWithTag("Player"));
			GameObject playerInstance= Instantiate(player, spawnPoint.transform.position, Quaternion.identity) as GameObject;
			playerInstance.name="Player";
            */
		}
	}

}