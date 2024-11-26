using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Rocket : Bullet
{
    public float explosionRadius;

    [SerializeField]
    GameObject explosion;

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        RaycastHit2D hit = Physics2D.CircleCast(transform.position, base.bulletRadius, transform.right, velocity * Time.deltaTime, ~(1 << gameObject.layer | base.bulletCollider.excludeLayers));
        Debug.DrawRay(transform.position, transform.right, Color.green, 1.0f);

        //Debug.Log(hit.distance);
        if (hit.collider != null)
        {
            Vector2 collisionPoint = hit.point + (hit.normal * base.bulletRadius);

            //Debug.DrawLine(hit.point, hit.point + hit.normal * base.bulletRadius, Color.blue, 1f);

            //Debug.Log(hit.point + " - " + (hit.normal * base.bulletRadius) + " : " + collisionPoint);
            transform.position = collisionPoint;
            Explode();
        }
    }

    public void Explode()
    {
        Collider2D[] inRad = Physics2D.OverlapCircleAll(transform.position, explosionRadius);
        foreach (Collider2D expCol in inRad)
        {
            Vector3 targetPosition = expCol.transform.position;
            //Debug.Log(gameObject.layer);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetPosition - transform.position, Vector2.Distance(transform.position, targetPosition), (1 << 10));
            Debug.DrawRay(transform.position, targetPosition - transform.position, Color.green, 1.0f);
            if (hit.collider != null)
            {
                Debug.Log(hit.point);
                Debug.Log(expCol.gameObject.name);
            }
            else
            {
                expCol.gameObject.BroadcastMessage("Damage", damage);

            }
        }
        GameObject effect = Instantiate(explosion);
        effect.transform.position = transform.position;
        effect.GetComponent<ExplosionController>().TriggerExplosion(explosionRadius);
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
