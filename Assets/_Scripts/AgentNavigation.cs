using UnityEngine;
using UnityEngine.AI;

public class RandomRoam : MonoBehaviour
{
    public Transform[] waypoints;    // Array of waypoints for patrol
    public float roamRadius = 10f;   // Radius within which the agent will roam
    public float roamDelay = 5f;     // Time delay between each roam
    public float waypointTolerance = 1f; // Distance to consider reaching a waypoint

    private NavMeshAgent agent;
    private Vector3 startPosition;   // Starting position of the agent
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startPosition = transform.position;

        agent.avoidancePriority = 50; // Default value, adjust as needed

        if (waypoints.Length > 0)
        {
            // Start patrolling between waypoints
            SetDestinationToWaypoint();
            InvokeRepeating("SetNextWaypoint", 0, roamDelay);
        }
        else
        {
            // Start random roaming within the defined radius
            InvokeRepeating("SetRandomDestination", 0, roamDelay);
        }
    }

    void SetRandomDestination()
    {
        // Get a random point within the specified radius
        Vector3 randomPoint = startPosition + Random.insideUnitSphere * roamRadius;
        randomPoint.y = startPosition.y; // Keep the Y coordinate consistent

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position); // Set the agent's destination
        }
    }

    void SetNextWaypoint()
    {
        // Check if the agent has reached the current waypoint
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < waypointTolerance)
        {
            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        SetDestinationToWaypoint();
    }

    void SetDestinationToWaypoint()
    {
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position); // Set the agent's destination to the current waypoint
        }
    }
}
