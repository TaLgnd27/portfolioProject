using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum State
{
    Attack,
    Teleport,
    Wait
}

public class TeleportBoss : Enemy
{
    private enum State
    {
        AttackCycle,
        Teleport,
        Wait
    }

    [SerializeField]
    GameObject[] teleports;
    //public RoomManager roomManager;

    [SerializeField]
    float attackDelay;

    [SerializeField]
    int shotsPerAttack = 4;
    int shotsRemaining;

    [SerializeField]
    int teleportsPerAttack;
    int teleportsRemaining;
    int previousTPIndex;

    State currentState = State.Teleport;

    public override void Start()
    {
        base.Start();
        teleportsPerAttack = teleports.Length+1;
    }

    public override void FixedUpdate()
    {
        if (!isSpawning)
        {
            base.FixedUpdate();
            switch (currentState)
            {
                case State.AttackCycle:
                    currentState = State.Wait;
                    StartCoroutine(AttackCycle());
                    //State nextState = Random.value > 0.5f ? State.Teleport : State.AttackCycle;
                    //StartCoroutine(Wait(attackDelay, nextState));
                    break;

                case State.Teleport:
                    currentState = State.Wait;
                    StartCoroutine(TeleportCycle());
                    break;

                case State.Wait:
                    break;
            }
        }
    }

    private IEnumerator AttackCycle()
    {
        if (shotsRemaining == 0)
        {
            shotsRemaining = shotsPerAttack;
        }
        yield return new WaitForSeconds(attackDelay);
        while (!base.Shoot()) {
            yield return new WaitForSeconds(attackDelay);
        }
        shotsRemaining--;
        if (shotsRemaining == 0)
        {
            //rb.MovePosition(center.transform.position);
            StartCoroutine(Wait(5f, State.Teleport));
        } else
        {
            StartCoroutine(Wait(1f, State.Teleport));
        }
    }

    private IEnumerator TeleportCycle()
    {
        if(teleportsRemaining == 0)
        {
            teleportsRemaining = teleportsPerAttack;
        }

        yield return new WaitForSeconds(0.5f);

        int tpIndex = Random.Range(0, teleports.Length);
        if (tpIndex == previousTPIndex)
        {
            tpIndex += Random.value > 0.5f ? 1 : teleports.Length + 1;
        }
        tpIndex %= teleports.Length;
        previousTPIndex = tpIndex;
        rb.transform.position = (teleports[tpIndex].transform.position);
        teleportsRemaining--;

        if (teleportsRemaining == 0)
        {
            StartCoroutine(AttackCycle());
        } else
        {
            StartCoroutine(TeleportCycle());
        }
    }

    private IEnumerator Wait(float duration, State nextState)
    {
        yield return new WaitForSeconds(duration);
        currentState = nextState;
    }
}
