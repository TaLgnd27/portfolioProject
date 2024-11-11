using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItemSet", menuName = "Level/ItemSet")]
public class ItemSet : ScriptableObject
{
    [SerializeField]
    public Item[] items;
}