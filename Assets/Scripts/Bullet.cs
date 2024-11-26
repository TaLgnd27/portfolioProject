using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocity;
    public int damage;
    public Rigidbody2D rb;
    public CircleCollider2D bulletCollider;

    public float bulletRadius;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<CircleCollider2D>();
        bulletRadius = (bulletCollider.radius * transform.localScale.y) + 0.05f;
    }
    public virtual void FixedUpdate()
    {
        rb.velocity = transform.right * velocity;
    }

    public virtual void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        collision.gameObject.BroadcastMessage("Damage", damage);
        Destroy(this.gameObject);
    }
}
