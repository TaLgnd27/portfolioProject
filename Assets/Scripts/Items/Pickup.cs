using UnityEngine;
using UnityEngine.Rendering;

public class Pickup : MonoBehaviour
{
    [SerializeField]
    public Gun gun;
    [SerializeField]
    public Item item;

    [SerializeField]
    private float checkRadius = 0.25f;

    [SerializeField]
    private float pushstep = 0.25f;

    public ItemHUD itemHUD;

    private void Start()
    {
        GameObject iHUDPrefab = Resources.Load<GameObject>("Prefabs/ItemHUD");
        itemHUD = Instantiate(iHUDPrefab, this.transform).GetComponent<ItemHUD>();
        if (item != null)
        {
            itemHUD.InitItem(item);
        }
        else if (gun != null)
        {
            itemHUD.InitGun(gun);
        }
        ResolveCollision();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Attempt Pickup");
        GameObject obj = collider.gameObject;
        Player player;
        if (obj.TryGetComponent<Player>(out player))
        {
            //Debug.Log(gun);
            if (gun != null)
            {
                Debug.Log("Attach Gun");
                player.gun.gun = gun;
            }

            Debug.Log(item);
            if (item != null)
            {
                player.AddItem(item);
            }
            Destroy(gameObject);
        }
    }

    private void ResolveCollision()
    {
        Vector2 position = transform.position;
        //float totalDistance = 0;

        while (Physics2D.OverlapCircle(position, checkRadius, 1 << 10))
        {
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            position += randomDir * pushstep;
            //totalDistance += pushstep;
            transform.position = position;
        }
    }
}
