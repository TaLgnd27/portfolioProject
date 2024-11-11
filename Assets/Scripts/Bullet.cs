using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float velocity;
    public int damage;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        rb.velocity = transform.right * velocity;
    }

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        collision.gameObject.BroadcastMessage("Damage", damage);
        Destroy(this.gameObject);
    }
}
