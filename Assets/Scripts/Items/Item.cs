using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Item : ScriptableObject
{
    [SerializeField]
    public string itemName;
    [SerializeField]
    public string itemDescription;
    [SerializeField]
    public Sprite itemSprite;

    public Creature owner;

    public virtual void OnPickup(Creature owner)
    {
        this.owner = owner;
    }

    public void OnRemove(Creature owner)
    {
        owner.RemoveItem(this);
        this.owner = null;
    }

    public void OnKill()
    {

    }

    public void OnHit()
    {

    }

    
}
