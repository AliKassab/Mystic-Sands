using UnityEngine;
using UnityEngine.AI;

public class RandomRoam : MonoBehaviour
{
    public Transform[] waypoints;            // Array of waypoints for patrol
    public float roamRadius = 10f;           // Radius within which the agent will roam
    public float roamDelay = 5f;             // Time delay between each roam
    public float waypointTolerance = 1f;     // Distance to consider reaching a waypoint

    private NavMeshAgent agent;
    private Vector3 startPosition;           // Starting position of the agent
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing.: " + gameObject.name);
            return;
        }

        if (!agent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent is not active.: " + gameObject.name);
            return;
        }

        startPosition = transform.position;

        // Ensure the agent is on a NavMesh surface
        if (NavMesh.SamplePosition(startPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position; // Snap agent to nearest point on NavMesh
        }
        else
        {
            Debug.LogError("Agent is not placed on a NavMesh.: " + gameObject.name);
            return;
        }

        agent.avoidancePriority = 50; // Default value, adjust as needed

        if (waypoints.Length > 0)
        {
            // Start patrolling between waypoints
            SetDestinationToWaypoint();
            InvokeRepeating(nameof(SetNextWaypoint), roamDelay, roamDelay);
        }
        else
        {
            // Set the initial random destination
            SetRandomDestination();
            InvokeRepeating(nameof(SetRandomDestination), roamDelay, roamDelay);
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
