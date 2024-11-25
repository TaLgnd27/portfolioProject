using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public EnemySet enemySet;
    public EnemySet bossSet;
    public Room room;

    private void Start()
    {
        enemySet = Resources.Load<EnemySet>("Lists/EnemySpawns");
        bossSet = Resources.Load<EnemySet>("Lists/BossSpawns");
        //SpawnEnemies();
    }

    public Enemy SpawnEnemies()
    {
        switch (room.type)
        {
            case RoomType.Normal:
                if (Random.value <= 0.25) { return null; }
                Debug.Log(enemySet.enemies.Length);
                GameObject enemy = enemySet.enemies[Random.Range(0, enemySet.enemies.Length)];
                Enemy instance = Object.Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>();
                instance.room = room;

                return instance;

            case RoomType.Boss:
                Debug.Log("Boss Spawn Attempt");
                GameObject bossFight = bossSet.enemies[Random.Range(0, bossSet.enemies.Length)];
                Enemy bossInstance = Object.Instantiate(bossFight, transform.position, Quaternion.identity).GetComponentInChildren<Enemy>();
                bossInstance.room = room;

                return bossInstance;
                //instance.Spawn();
        }
        return null;
    }
}
