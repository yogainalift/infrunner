using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public Transform spawnPoint;
	public GameObject player;
	private GameCamera cam;


	void Start () {
        cam = GetComponent<GameCamera>();
        if (spawnPoint == null)
        {
            spawnPoint = GameObject.Find("SpawnPoint").transform;
        }
    
		SpawnPlayer();
        //cam.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, this.transform.position.z);
    }
	
	public void SpawnPlayer(){
		GameObject playerInstance = (Instantiate(player, spawnPoint.transform.position, Quaternion.identity)) as GameObject;
		playerInstance.name="Player";
		cam.SetTarget(playerInstance.transform);
		
	}
}
