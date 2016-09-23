using UnityEngine;

public class PlayerRespawn : MonoBehaviour {

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {

	}

    void respawn()
    {
        // Player respawns at (0,0,0)
        transform.position = Vector3.zero;

        // Kill all instances of Rock
        Object[] allObjects = FindObjectsOfType(typeof(GameObject));
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.name == "Rock(Clone)")
            {
                Destroy(obj);
            }
        }
    }

    void OnCollisionEnter(Collision col)
    {
        // If the player collides with an instance of Rock then he respawns
        if (col.gameObject.name == "Rock(Clone)")
        {
            respawn();
        }
    }
}
