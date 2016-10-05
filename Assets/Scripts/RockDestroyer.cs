using UnityEngine;

public class RockDestroyer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        // Get tha player's current position
        var playerCurrentPosition = GameObject.Find("Player").transform.position.z;
        // If the player has passed this object and it has a parent then it gets destroyed
        if ( playerCurrentPosition - this.transform.position.z > 10 && this.gameObject.name != "Rock" )
        {
            Destroy(this.gameObject);
        }
    }
}
