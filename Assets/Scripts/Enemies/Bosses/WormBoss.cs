using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class WormBoss : Enemy
{
    [SerializeField]
    public GameObject bossVisual;

    [SerializeField]
    public GameObject corner1;

    [SerializeField]
    public GameObject corner2;

    public Vector2 jumpTarget;

    private enum State
    {
        Burrow,
        Jump,
        Shoot,
        Wait
    }

    private State state;

    // Start is called before the first frame update
    public override void OnSpawn()
    {
        state = State.Burrow;
        StartCoroutine(Burrow());
    }

    public override void FixedUpdate()
    {
        if (!base.isSpawning)
        {
            base.FixedUpdate();
            switch (state)
            {
                case State.Jump:
                    if (!agent.pathPending && agent.remainingDistance <= 0.1)
                    {
                        // The agent has reached the destination
                        OnDestinationReached();
                    }
                    break;
            }
        }
    }

    private IEnumerator Burrow()
    {
        state = State.Burrow;
        bossVisual.SetActive(false);
        yield return new WaitForSeconds(Random.Range(1f, 2f));
        bossVisual.SetActive(true);

        transform.position = GetRandomPositionWithinBoundingBox(corner1.transform.position, corner2.transform.position);

        //Select Next State
        int randomChoice = Random.Range(0, 2);
        if (randomChoice == 0)
        {
            Jump();
        } else
        {
            StartCoroutine(ShootPhase());
        }
    }

    private void Jump()
    {
        state = State.Jump;
        jumpTarget = Player.Instance.transform.position;

        agent.SetDestination(jumpTarget);
        Debug.Log(agent.destination);
    }

    private IEnumerator ShootPhase()
    {
        yield return new WaitForSeconds(0.5f);
        base.Shoot();
        yield return new WaitForSeconds(0.5f);
        base.Shoot();
        yield return new WaitForSeconds(0.5f);
        base.Shoot();

        yield return new WaitForSeconds(2f);

        state = State.Burrow;
        StartCoroutine(Burrow());
    }



    void OnDestinationReached()
    {
        agent.ResetPath();
        StartCoroutine(Burrow());
    }


    Vector2 GetRandomPositionWithinBoundingBox(Vector2 corner1, Vector2 corner2)
    {
        // Calculate the minimum and maximum x and y values for the bounding box
        float minX = Mathf.Min(corner1.x, corner2.x);
        float maxX = Mathf.Max(corner1.x, corner2.x);

        float minY = Mathf.Min(corner1.y, corner2.y);
        float maxY = Mathf.Max(corner1.y, corner2.y);

        // Generate a random position within the bounding box
        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }
}
