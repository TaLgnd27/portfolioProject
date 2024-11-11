using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ModifierType
{
    Additive,
    Multiplicative
}
public class StatModifier
{
    public float value {  get; private set; }
    public ModifierType type { get; private set; }
    public int order { get; private set; }

    public StatModifier(float value, ModifierType type, int order = 0)
    {
        this.value = value;
        this.type = type;
        this.order = order;
    }
}
