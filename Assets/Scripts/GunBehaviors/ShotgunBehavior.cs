using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunBehavior : GunBehavior
{
    [SerializeField]
    int arc = 90;
    [SerializeField]
    int shots = 1;

    public override void ReloadGun()
    {
        arc = base.gun.arc;
        shots = base.gun.shots;
        base.ReloadGun();
    }

    public override bool Shoot(Quaternion direction, Transform spawnpoint)
    {
        if (!base.cooldown)
        {
            float offset = arc / shots;
            for (int i = -shots/2; i <= shots/2; i++)
            {
                if (UnityEngine.Random.value >= gun.accuracy)
                {
                    float randomVariation = UnityEngine.Random.Range(-gun.spread, gun.spread);
                    direction *= Quaternion.Euler(0f, 0f, randomVariation);
                }
                Debug.Log(offset * i);
                Quaternion offsetDirection = direction * Quaternion.Euler(0f, 0f, offset * i);
                GameObject spawned = Instantiate(gun.bullet, spawnpoint.position, offsetDirection);
                spawned.GetComponent<Bullet>().velocity = gun.velocity;
                spawned.GetComponent<Bullet>().damage = gun.damage;
                //spawned.transform.rotation = direction;
                spawned.GetComponent<SpriteRenderer>().color = base.color;
                spawned.GetComponent<Collider2D>().excludeLayers |= 1 << gameObject.layer;
            }
            base.cooldown = true;

            audioSource.Play();
            StartCoroutine("Cooldown", gun.rof);
            return true;
        }
        return false;
    }
}
