using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSpawner : MonoBehaviour
{
    [SerializeField]
    Sprite defaultSprite;

    GunSet guns;
    // Start is called before the first frame update
    void Start()
    {
        guns = Resources.Load<GunSet>("Lists/GunSet");

        Gun gun = guns.guns[Random.Range(0, guns.guns.Length)];
        GameObject instance = Object.Instantiate(new GameObject(gun.name), transform.position, Quaternion.identity);
        instance.layer = 9;
        Debug.Log(instance);

        SpriteRenderer sp = instance.AddComponent<SpriteRenderer>();
        if (gun.gunSprite != null)
        {
            sp.sprite = gun.gunSprite;
        }
        else
        {
            sp.sprite = defaultSprite;
        }

        Pickup p = instance.AddComponent<Pickup>();
        p.gun = gun;

        instance.AddComponent<PolygonCollider2D>().isTrigger = true;
    }
}
