using UnityEngine;

public class RockGenerator : MonoBehaviour {

    int minDistance = 50;  // the minimum distance in z between 2 rocks
    float zPreviousRock = 0;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        // Get the player's current position
        var playerCurrentPosition = this.transform.position.z;
        GenerateRock(playerCurrentPosition + 120);
        if (playerCurrentPosition == 0)
        {
            zPreviousRock = 0;
        }
	}

    void GenerateRock(float z)
    {
        if (z - zPreviousRock > minDistance) {
            // The X position of Vector3 can take one of these values: (-3, 0, 3)
            Vector3 position = new Vector3(Random.Range(-1, 2) * 2.7f, 0.5f, z);
            Instantiate(GameObject.Find("Rock"), position, Quaternion.identity);
            zPreviousRock = position.z;
        }
    }
}
 