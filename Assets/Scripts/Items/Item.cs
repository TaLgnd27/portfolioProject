using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Pickups/Item")]
public class Item : ScriptableObject
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public string itemDescription;
    [SerializeField]
    public Sprite itemSprite;

    [SerializeField]
    int healthMod;
    StatModifier healthModifier;
    [SerializeField]
    float speedMod;
    StatModifier speedModifier;

    public Creature owner;

    //public ItemBehavior behavior;   

    public virtual void OnPickup(Creature owner)
    {
        this.owner = owner;
        //behavior.OnPickup();
        if(healthMod != 0)
        {
            healthModifier = new StatModifier(healthMod, ModifierType.Additive);
            owner.maxHP.AddModifier(healthModifier);
            if (healthMod > 0)
            {
                owner.Heal(healthMod);
            }
            else
            {
                owner.Damage(healthMod);
            }
        }
        if(speedMod != 0)
        {
            speedModifier = new StatModifier(speedMod, ModifierType.Additive);
            owner.speed.AddModifier(speedModifier);
        }
    }

    public void OnRemove(Creature owner = null)
    {
        if (owner != null)
        {
            owner.RemoveItem(this);
        } else
        {
            owner = this.owner;
        }

        if (healthMod != 0)
        {
            owner.maxHP.RemoveModifier(healthModifier);
            owner.Damage(healthMod);
        }
        if (speedMod != 0)
        {
            owner.speed.RemoveModifier(speedModifier);
        }

        this.owner = null;
    }

    public void OnKill()
    {

    }

    public void OnHit()
    {

    }
}
