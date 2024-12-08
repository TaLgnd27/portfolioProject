using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField]
    int hp;

    [SerializeField]
    private float checkRadius = 0.25f;

    [SerializeField]
    private float pushstep = 0.05f;

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
        Debug.Log("Resolving Collision");
        Vector2 position = transform.position;
        //float totalDistance = 0;

        while (Physics2D.OverlapCircle(position, checkRadius, 1 << 10))
        {
            Debug.Log("Overlapping Layer 10");
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            position += randomDir * pushstep;
            //totalDistance += pushstep;

            transform.position = position;
        }
    }
}
