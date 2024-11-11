using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stat
{
    public float baseValue;
    private readonly List<StatModifier> modifiers = new List<StatModifier>();

    // Start is called before the first frame update
    public Stat(float baseValue)
    {
        this.baseValue = baseValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        modifiers.Add(modifier);
        modifiers.Sort((a,b) => a.order.CompareTo(b.order));
    }

    public void RemoveModifier(StatModifier modifier)
    {
        Debug.Log("Attempt Removal");
        modifiers.Remove(modifier);
    }

    public float GetModifiedValue()
    {
        float finalValue = baseValue;

        foreach (StatModifier modifier in modifiers)
        {
            if (modifier.type == ModifierType.Additive)
            {
                finalValue += modifier.value;
            }
            else if (modifier.type == ModifierType.Multiplicative)
            {
                finalValue *= modifier.value;
            }
        }
        return finalValue;
    }
}
