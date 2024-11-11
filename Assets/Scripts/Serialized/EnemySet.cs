using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemySet", menuName = "Level/EnemySet")]
public class EnemySet : ScriptableObject
{
    [SerializeField]
    public GameObject[] enemies;
}