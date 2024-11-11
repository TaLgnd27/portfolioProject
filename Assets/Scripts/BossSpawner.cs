using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    EnemySet bossSet;

    private void Start()
    {
        bossSet = Resources.Load<EnemySet>("Lists/EnemySpawns");
        //SpawnEnemies();
    }
}
