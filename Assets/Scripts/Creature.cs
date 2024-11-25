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
    private float dashCountBase = 1;
    Stat dashCount;

    [SerializeField]
    private float dashMultBase = 4;
    Stat dashMult;

    [SerializeField]
    private float dashDurationBase = 0.3f;
    Stat dashDuration;

    public int startLayer;
    [SerializeField]
    public int dodgeLayer = 3;

    [SerializeField]
    private ParticleSystem dodgeParticle;

    private StatModifier dashModifier;

    List<Item> items = new List<Item>();
    List<StatModifier> dashCountMods = new List<StatModifier>();

    public bool isDead = false;

    public delegate void onHealthChangeEvent(float percent);
    public event onHealthChangeEvent onHealthChange;

    [SerializeField]
    private float iTimeBase = 0;
    public Stat iTime;
    public bool isInvuln = false;

    public SpriteRenderer spriteRenderer;

    //public float GetSpeed() { return speed; }

    public virtual void Awake()
    {
        startLayer = gameObject.layer;

        maxHP = new Stat(maxHPBase);
        speed = new Stat(speedBase);
        dashCooldown = new Stat(dashCooldownBase);
        dashMult = new Stat(dashMultBase);
        dashDuration = new Stat(dashDurationBase);
        dashCount = new Stat(dashCountBase);
        iTime = new Stat(iTimeBase);

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (gun == null)
        {
            gun = gameObject.AddComponent<GunBehavior>();
        }
        //gun.Init("test", 10, 0.5f, 1, this);
        hp = (int) maxHP.GetModifiedValue();
        rb = gameObject.GetComponent<Rigidbody2D>();

        Debug.Log("Checking health change");
        if (onHealthChange != null)
        {
            Debug.Log("Sending hp update");
            onHealthChange(hp / maxHP.GetModifiedValue());
        }
    }

    public virtual void Heal(int hp)
    {
        this.hp += hp;
        this.hp = Math.Min((int) maxHP.GetModifiedValue(), this.hp);

        if (onHealthChange != null)
        {
            onHealthChange((float)this.hp / maxHP.GetModifiedValue());
        }
    }

    public void RequestHealthUpdate()
    {
        if (onHealthChange != null)
        {
            onHealthChange((float)this.hp / maxHP.GetModifiedValue());
        }
    }

    public virtual void Damage(int hp)
    {
        this.hp -= hp;
        if (this.hp <= 0)
        {
            if (!isDead)
            {
                Debug.Log("Should be dead");
                Death();
            }
        }

        if (onHealthChange != null)
        {
            onHealthChange((float)this.hp / maxHP.GetModifiedValue());
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
        if ((!isDashing && !isDashCooldown) || (!isDashing && dashCount.GetModifiedValue() > 0))
        {
            isDashing = true;
            gameObject.layer = dodgeLayer;
            dashModifier = new StatModifier(dashMult.GetModifiedValue(), ModifierType.Multiplicative);
            Color color = spriteRenderer.color;
            color.a = 0.1f;
            spriteRenderer.color = color;
            speed.AddModifier(dashModifier);
            if(dodgeParticle != null)
            {
                dodgeParticle.Play();
            }
            StartCoroutine("Dashing", dashDuration.GetModifiedValue());
        }
    }

    public IEnumerator Dashing(float duration)
    {
        StatModifier dashCountMod = new StatModifier(-1, ModifierType.Additive);
        dashCountMods.Add(dashCountMod);
        dashCount.AddModifier(dashCountMod);
        yield return new WaitForSeconds(duration);
        speed.RemoveModifier(dashModifier);
        isDashing = false;
        isDashCooldown = true;
        StartCoroutine("DashCooldown", dashCooldown.GetModifiedValue());
        gameObject.layer = startLayer;
        Color color = spriteRenderer.color;
        color.a = 1; 
        spriteRenderer.color = color;
    }

    private IEnumerator DashCooldown(float duration)
    {
        yield return new WaitForSeconds(duration);
        isDashCooldown = false;
        dashCount.RemoveModifier(dashCountMods.First());
        dashCountMods.Remove(dashCountMods.First());
    }

    public virtual void Death()
    {
        Debug.Log("Base death ran");
        isDead = true;
        Destroy(this.gameObject);
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
