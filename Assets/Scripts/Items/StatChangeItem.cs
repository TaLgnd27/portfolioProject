using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewStatItem", menuName = "Pickups/Stat Item")]
public class StatChangeItem : Item
{
    [SerializeField]
    float speedMod = 0f;

    [SerializeField]
    float rofMod = 0f;

    public override void OnPickup(Creature owner)
    {
        base.OnPickup(owner);
    }
}
