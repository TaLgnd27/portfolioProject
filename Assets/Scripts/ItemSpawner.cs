using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    Sprite defaultSprite;

    ItemSet items;
    // Start is called before the first frame update
    void Start()
    {
        items = Resources.Load<ItemSet>("Lists/ItemSet");

        Item item = items.items[Random.Range(0, items.items.Length)];
        GameObject instance = Object.Instantiate(new GameObject(item.name), transform.position, Quaternion.identity);

        SpriteRenderer sp = instance.AddComponent<SpriteRenderer>();
        if (item.itemSprite != null)
        {
            sp.sprite = item.itemSprite;
        } else
        {
            sp.sprite = defaultSprite;
        }

        Pickup p = instance.AddComponent<Pickup>();
        p.item = item;

        instance.AddComponent<PolygonCollider2D>().isTrigger = true;
    }
}
