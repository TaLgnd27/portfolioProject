using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Creature
{
    public GameObject player;
    public bool isSpawning;
    public Room room;

    public delegate void onDeathEvent();
    public event onDeathEvent OnDeath;
    public NavMeshAgent agent;

    public bool hasLOS = false;

    public virtual void Start()
    {
        isSpawning = true;
        StartCoroutine("SpawnDelay", 1);
        player = FindObjectOfType<Player>().gameObject;

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = base.speed.GetModifiedValue() * Time.fixedDeltaTime;
    }

    public virtual void FixedUpdate()
    {
        if (!isSpawning)
        {
            //Vector2 moveDir = getMoveDir();
            //base.Move(moveDir);
            base.Look(player.transform.position);
            base.Turn();

            hasLOS = base.gun.CanHit(player);
            //base.Shoot();
        }
    }

    Vector2 getMoveDir()
    {
        Vector2 dir = Vector2.zero;
        float dist = Vector3.Distance(player.transform.position, transform.position);
        dir = Vector2.Perpendicular(transform.position - player.transform.position);

        if (dist < 4)
        {
            dir += (Vector2)(transform.position - player.transform.position);
        }

        // Find all colliders within the check radius
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.75f);
        Debug.Log("Colliders detected: " + colliders.Length);
        // Loop through each collider detected
        foreach (Collider2D collider in colliders)
        {
            // Ignore self-collisions
            if (collider.gameObject == gameObject || collider.gameObject.layer == 7) continue;
            Debug.Log(collider.transform.position);
            // Calculate direction away from the collider
            Vector2 directionAway = (Vector2)transform.position - collider.ClosestPoint(transform.position);
            dir += directionAway * 10; // Accumulate directions
        }
        return dir.normalized;
    }

    public override void Death()
    {
        if (OnDeath != null)
        {
            OnDeath();
        }
        base.Death();
    }

    private IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isSpawning = false;
    }

    public virtual IEnumerator Pathfinding(Transform target)
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            if (!isSpawning)
            {
                agent.SetDestination(target.position);
            }
        }
    }
}