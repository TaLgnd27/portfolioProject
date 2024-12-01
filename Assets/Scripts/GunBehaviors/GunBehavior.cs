using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GunBehavior : MonoBehaviour
{
    [SerializeField]
    public Gun gun;
    
    private CircleCollider2D bulletCollider;
    public Creature owner;

    public bool cooldown = false;
    public Color color;

    public AudioSource audioSource;

    public void Start()
    {
        if (!gun)
        {
            gun = Resources.Load<Gun>("Guns/DefaultGun");
        }
        color = this.GetComponent<SpriteRenderer>().color;
        ReloadGun();
    }

    public virtual void ReloadGun()
    {
        bulletCollider = gun.bullet.GetComponent<CircleCollider2D>();
        if(audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.playOnAwake = false;
        audioSource.clip = gun.sound;
    }

    public virtual bool Shoot(Quaternion direction, Transform spawnpoint)
    {
        if (!cooldown)
        {
            if (UnityEngine.Random.value >= gun.accuracy)
            {
                float randomVariation = UnityEngine.Random.Range(-gun.spread, gun.spread);
                direction *= Quaternion.Euler(0f, 0f, randomVariation);
            }
            GameObject spawned = Instantiate(gun.bullet, spawnpoint.position, direction);
            spawned.GetComponent<Bullet>().velocity = gun.velocity;
            spawned.GetComponent<Bullet>().damage = gun.damage;
            spawned.transform.rotation = direction;
            spawned.GetComponent<SpriteRenderer>().color = color;
            spawned.GetComponent<Collider2D>().excludeLayers |= 1 << gameObject.layer;
            cooldown = true;

            StartCoroutine("Cooldown", gun.rof);

            audioSource.Play();
            return true;
        }
        return false;
    }

    public bool CanHit(GameObject target)
    {
        RaycastHit2D hit = Physics2D.CircleCast(transform.position, (bulletCollider.radius * gun.bullet.transform.localScale.y) + 0.05f, target.transform.position - transform.position, Mathf.Infinity, ~(1 << gameObject.layer | 1 << 7));
        if (hit.collider.gameObject.layer == target.gameObject.layer)
        {
            return hit;
        }
        else
        {
            return false;
        }
    }

    private IEnumerator Cooldown(float delay)
    {
        yield return new WaitForSeconds(delay);
        cooldown = false;
    }
}
