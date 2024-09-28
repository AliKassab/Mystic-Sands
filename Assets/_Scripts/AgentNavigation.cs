using UnityEngine;
using UnityEngine.AI;

public class RandomRoam : MonoBehaviour
{
    public Transform[] waypoints;           // Array of waypoints for patrol
    public float roamRadius = 10f;          // Radius within which the agent will roam
    public float roamDelay = 5f;            // Time delay between each roam
    public float waypointTolerance = 1f;    // Distance to consider reaching a waypoint

    private NavMeshAgent agent;
    private Vector3 startPosition;
    private int currentWaypointIndex = 0;
    private float roamTimer = 0f;           // Timer for roaming logic
    private float nextRoamTime;             // Randomized next roam delay

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null || !agent.isActiveAndEnabled)
        {
            Debug.LogError("NavMeshAgent component is missing or inactive.: " + gameObject.name);
            return;
        }

        startPosition = transform.position;

        if (NavMesh.SamplePosition(startPosition, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError("Agent is not placed on a NavMesh.: " + gameObject.name);
            return;
        }

        agent.avoidancePriority = 50;
        nextRoamTime = roamDelay + Random.Range(0f, 1f); // Randomize roam delay to distribute workload

        if (waypoints.Length > 0)
        {
            SetDestinationToWaypoint();
        }
        else
        {
            SetRandomDestination();
        }
    }

    void Update()
    {
        roamTimer += Time.deltaTime;

        if (roamTimer >= nextRoamTime)
        {
            // Update the agent destination based on its current behavior
            if (waypoints.Length > 0)
            {
                SetNextWaypoint();
            }
            else
            {
                SetRandomDestination();
            }

            roamTimer = 0f;
            nextRoamTime = roamDelay + Random.Range(0f, 1f); // Re-randomize delay for the next update
        }
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = startPosition + Random.insideUnitSphere * roamRadius;
        randomPoint.y = startPosition.y;

        if (NavMesh.SamplePosition(randomPoint, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }

    void SetNextWaypoint()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < waypointTolerance)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
        SetDestinationToWaypoint();
    }

    void SetDestinationToWaypoint()
    {
        if (waypoints.Length > 0)
        {
            agent.SetDestination(waypoints[currentWaypointIndex].position);
        }
    }
}
