using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class BurstGun : GunBehavior
{
    public override bool Shoot(Quaternion direction, Transform spawnpoint)
    {
        if (!base.cooldown)
        {
            if (UnityEngine.Random.value >= gun.accuracy)
            {
                float randomVariation = UnityEngine.Random.Range(-gun.spread, gun.spread);
                Debug.Log(randomVariation);
                direction *= Quaternion.Euler(0f, 0f, randomVariation);
            }


            float offset = 360 / 8;
            for (int i = 0; i < 8; i++)
            {
                direction *= Quaternion.Euler(0f, 0f, offset*i);
                GameObject spawned = Instantiate(gun.bullet, spawnpoint.position, direction);
                spawned.GetComponent<Bullet>().velocity = gun.velocity;
                spawned.GetComponent<Bullet>().damage = gun.damage;
                spawned.transform.rotation = direction;
                spawned.GetComponent<SpriteRenderer>().color = base.color;
                spawned.GetComponent<Collider2D>().excludeLayers |= 1 << gameObject.layer;
            }
            base.cooldown = true;

            StartCoroutine("Cooldown", gun.rof);
            return true;
        }
        return false;
    }
}
