using UnityEngine;
using System.Collections.Generic;

public class LOSPathfinding : MonoBehaviour
{
    public Transform target;          // Target to maintain LoS with
    public LayerMask obstacleLayer;    // Layer for obstacles to block LoS
    public float losCheckInterval = 0.2f; // Time between LoS checks
    public float waypointThreshold = 0.5f; // Distance to consider a waypoint reached

    private List<Vector2> path = new List<Vector2>(); // Current path
    private int currentWaypoint = 0;   // Index of current waypoint in path

    void Start()
    {
        InvokeRepeating(nameof(UpdateLineOfSight), 0f, losCheckInterval);
        CalculatePathToTarget(); // Initial path calculation
    }

    void Update()
    {
        FollowPath();
    }

    private void UpdateLineOfSight()
    {
        // Check if there's a clear LoS to the target
        if (HasLineOfSight())
        {
            // Directly move towards the target
            MoveTowardsTarget();
        }
        else
        {
            // No LoS, calculate a new path
            CalculatePathToTarget();
        }
    }

    private bool HasLineOfSight()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float distance = Vector2.Distance(transform.position, target.position);

        // Check for obstacles in the line to the target
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, distance, obstacleLayer);
        return hit.collider == null; // True if no obstacles
    }

    private void CalculatePathToTarget()
    {
        // Replace with your pathfinding algorithm to calculate a path to the target
        //path = AStarPathfinding.CalculatePath(transform.position, target.position);
        currentWaypoint = 0; // Reset waypoint index
    }

    private void FollowPath()
    {
        // If the path is empty, no need to move
        if (path == null || path.Count == 0) return;

        // Check if we've reached the current waypoint
        Vector2 targetPosition = path[currentWaypoint];
        if (Vector2.Distance(transform.position, targetPosition) < waypointThreshold)
        {
            currentWaypoint++; // Move to the next waypoint

            // Check if we reached the end of the path
            if (currentWaypoint >= path.Count)
            {
                path.Clear(); // Clear the path if finished
                return;
            }
        }

        // Move towards the current waypoint
        MoveTowards(path[currentWaypoint]);
    }

    private void MoveTowards(Vector2 destination)
    {
        float speed = 5f; // Movement speed
        Vector2 position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);
        transform.position = position;
    }

    private void MoveTowardsTarget()
    {
        // Move directly towards the target
        MoveTowards(target.position);
    }
}
