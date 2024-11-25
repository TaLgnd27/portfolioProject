using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Rocket : Bullet
{
    public float explosionRadius;

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        Collider2D[] inRad = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D expCol in inRad)
        {
            expCol.gameObject.BroadcastMessage("Damage", damage);
        }
        Destroy(this.gameObject);
    }
}
