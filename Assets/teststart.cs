using System.Collections;
using UnityEngine;

public class TestStart : MonoBehaviour
{
    public float targetYPosition = 5f; // Target Y position
    public float speed = 2f;           // Speed of movement
    private bool shouldMove = false;   // Flag to control movement

    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            shouldMove = true; // Start moving when the player is in the trigger
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldMove)
        {
            // Move the object towards the target position
            Vector3 targetPosition = new Vector3(transform.position.x, targetYPosition, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, targetPosition, speed * Time.deltaTime);

            // Stop moving if close enough to the target position
            if (Mathf.Abs(transform.position.y - targetYPosition) < 0.01f)
            {
                shouldMove = false;
            }
        }
    }
}
