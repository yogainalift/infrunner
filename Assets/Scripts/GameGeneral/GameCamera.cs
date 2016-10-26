using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour
{

    private Transform target;
    private float trackSpeed = 11.5f;

    public void SetTarget(Transform t)
    {
        target = t;
    }

    public void SetFastTarget(Transform t)
    {
        trackSpeed = 70;
    }

    void LateUpdate()
    {
        if (target)
        {
            float x = IncrementTowards(transform.position.x, target.position.x, trackSpeed);
            float y = IncrementTowards(transform.position.y, target.position.y + 3, trackSpeed);
            transform.position = new Vector3(x, y, transform.position.z);
        }
        if (target == null)
        {
            StartCoroutine(waitFor(2f));
            
        }

    }

    private float IncrementTowards(float n, float target, float a)
    {
        if (n == target)
        {
            trackSpeed = 11.5f; //This is not working 100% correct but fine
            return n;
        }
        else
        {
            float dir = Mathf.Sign(target - n);
            n += a * Time.deltaTime * dir;
            return (dir == Mathf.Sign(target - n)) ? n : target;
        }
    }
    IEnumerator waitFor(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (!GameObject.Find("Player"))
            this.GetComponent<GameManager>().SpawnPlayer();
        SetFastTarget(GameObject.Find("Player").transform);
    }
}
