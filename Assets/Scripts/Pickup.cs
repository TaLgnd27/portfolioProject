using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    public Gun gun;
    [SerializeField]
    public Item item;

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Attempt Pickup");
        GameObject obj = collider.gameObject;
        Player player;
        if (obj.TryGetComponent<Player>(out player))
        {
            Debug.Log(gun);
            if (gun != null)
            {
                Debug.Log("Attach Gun");
                player.gun.gun = gun;
            }

            if (item)
            {
                player.AddItem(item);
            }
            Destroy(gameObject);
        }
    }
}
