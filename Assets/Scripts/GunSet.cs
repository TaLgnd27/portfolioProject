using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGunSet", menuName = "Level/GunSet")]
public class GunSet : ScriptableObject
{
    [SerializeField]
    public Gun[] guns;
}