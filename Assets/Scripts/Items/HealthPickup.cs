using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    int hp;

    [SerializeField]
    private float checkRadius = 0.75f;

    [SerializeField]
    private float pushstep = 0.75f;

    private void Start()
    {
        ResolveCollision();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Attempt Pickup");
        GameObject obj = collider.gameObject;
        Player player;
        if (obj.TryGetComponent<Player>(out player))
        {
            obj.BroadcastMessage("Heal", hp);
            Destroy(gameObject);
        }
    }

    private void ResolveCollision()
    {
        Vector2 position = transform.position;
        float totalDistance = 0;

        while (Physics2D.OverlapCircle(position, checkRadius, 10))
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            position += randomDir * pushstep;
            totalDistance += pushstep;

            transform.position = position;
        }
    }
}
