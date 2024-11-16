using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Creature : MonoBehaviour
{
    [SerializeField]
    private int maxHPBase = 5;
    public Stat maxHP;
    [SerializeField]
    public int hp;

    [SerializeField]
    private float speedBase = 150;
    public Stat speed;

    [SerializeField]
    public GunBehavior gun;
    public Rigidbody2D rb;

    public Vector2 lookTarget;
    [SerializeField]
    public GameObject spawnpoint;

    public bool isDashing = false;
    [SerializeField]

    private float dashCooldownBase = 3;
    public Stat dashCooldown;
    public bool isDashCooldown = false;

    [SerializeField]
    private float dashMultBase = 4;
    Stat dashMult;

    [SerializeField]
    private float dashDurationBase = 0.3f;
    Stat dashDuration;

    int oldLayer;
    [SerializeField]
    int dodgeLayer = 3;

    private StatModifier dashModifier;

    List<Item> items = new List<Item>();


    //public float GetSpeed() { return speed; }

    public virtual void Awake()
    {
        maxHP = new Stat(maxHPBase);
        speed = new Stat(speedBase);
        dashCooldown = new Stat(dashCooldownBase);
        dashMult = new Stat(dashMultBase);
        dashDuration = new Stat(dashDurationBase);

        if (gun == null)
        {
            gun = gameObject.AddComponent<GunBehavior>();
        }
        //gun.Init("test", 10, 0.5f, 1, this);
        hp = (int) maxHP.GetModifiedValue();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    public virtual void Heal(int hp)
    {
        this.hp += hp;
        this.hp = Math.Min((int) maxHP.GetModifiedValue(), hp);
    }

    public virtual void Damage(int hp)
    {
        this.hp -= hp;
        if (this.hp <= 0)
        {
            Death();
        }
    }

    public void Move(Vector2 moveInput)
    {
        //moveInput = Quaternion.Inverse(transform.rotation) * new Vector3(moveInput.x, moveInput.y, 0);
        rb.velocity = moveInput * speed.GetModifiedValue() * Time.fixedDeltaTime;
    }

    public void Turn()
    {
        Vector3 lookPos = lookTarget - (Vector2)transform.position;
        float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
        var rotation = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1));
        transform.rotation = rotation;
    }

    public void Look(Vector2 target)
    {
        lookTarget = target;

    }

    public virtual bool Shoot()
    {
        Quaternion rotation = transform.rotation;
        return gun.Shoot(rotation, spawnpoint.transform);
    }

    public void Dash(Vector2 moveInput)
    {
        if (!isDashing && !isDashCooldown)
        {
            isDashing = true;
            oldLayer = gameObject.layer;
            gameObject.layer = dodgeLayer;
            dashModifier = new StatModifier(dashMult.GetModifiedValue(), ModifierType.Multiplicative);
            speed.AddModifier(dashModifier);
            StartCoroutine("Dashing", dashDuration.GetModifiedValue());
        }
    }

    public IEnumerator Dashing(float duration)
    {
        yield return new WaitForSeconds(duration);
        speed.RemoveModifier(dashModifier);
        isDashing = false;
        isDashCooldown = true;
        StartCoroutine("DashCooldown", dashCooldown.GetModifiedValue());
        gameObject.layer = oldLayer;
    }

    private IEnumerator DashCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDashCooldown = false;
    }

    public virtual void Death()
    {
        Destroy(gameObject);
    }

    public void AddItem(Item item)
    {
        Debug.Log(item);
        items.Append<Item>(item);
        item.OnPickup(this);
    }

    public void RemoveItem(Item item)
    {
        item.OnRemove();
        items.Remove(item);
    }
}
