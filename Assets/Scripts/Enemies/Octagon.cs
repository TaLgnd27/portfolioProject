using UnityEngine;

public class Octagon : Enemy
{
    public override void Start()
    {
        base.Start();
        //StartCoroutine("Pathfinding", base.player.transform);
    }

    public override void FixedUpdate()
    {
        if (!isSpawning)
        {
            //Debug.Log(speed.GetModifiedValue());
            //Vector2 moveDir = getMoveDir();
            //base.Move(moveDir);
            transform.rotation *= Quaternion.Euler(0f, 0f, base.speed.GetModifiedValue());

            hasLOS = base.gunBehavior.CanHit(player);
            base.Shoot();
        }
    }
}
