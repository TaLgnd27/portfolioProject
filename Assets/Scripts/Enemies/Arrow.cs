using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Arrow : Enemy
{
    public override void Start()
    {
        base.Start();
        //StartCoroutine("Pathfinding", base.player.transform);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!base.hasLOS)
        {
            agent.SetDestination(transform.position);
            StopCoroutine("Pathfinding");
        }
        else
        {
            base.Shoot();
            base.StartCoroutine("Pathfinding", base.player.transform);
            //Pathfinding(base.player.transform);
        }
    }
}
