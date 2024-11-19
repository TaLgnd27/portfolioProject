using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEditor.Progress;

public class RoomManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Tilemap tilemap; // Reference to the Tilemap
    public TileBase tile;   // Reference to the Tile you want to place
    public Room room;
    public GameObject map;
    public GameObject[] spawners;
    public int enemyCount = 0;
    public Player player;
    public LevelManager levelManager;
    public HudManager hudManager;
    [SerializeField]
    private GameObject[] doors;
    //public GameObject mapImage;

    void Start()
    {
        player = FindAnyObjectByType<Player>();
        levelManager = FindAnyObjectByType<LevelManager>();
        hudManager = FindAnyObjectByType<HudManager>();
        player.onTransitionDone += CloseDoors;
        BuildDoors();
    }

    void BuildDoors()
    {
        if((room.doors & 1 << 0) != 0)
        {
            tilemap.SetTile(new Vector3Int(0, 4, 0), null);
            tilemap.SetTile(new Vector3Int(-1, 4, 0), null);
            doors[0].SetActive(true);
        }
        if ((room.doors & 1 << 1) != 0)
        {
            tilemap.SetTile(new Vector3Int(0, -5, 0), null);
            tilemap.SetTile(new Vector3Int(-1, -5, 0), null);
            doors[1].SetActive(true);
        }
        if ((room.doors & 1 << 2) != 0)
        {
            tilemap.SetTile(new Vector3Int(8, 0, 0), null);
            tilemap.SetTile(new Vector3Int(8, -1, 0), null);
            doors[2].SetActive(true);
        }
        if ((room.doors & 1 << 3) != 0)
        {
            tilemap.SetTile(new Vector3Int(-9, 0, 0), null);
            tilemap.SetTile(new Vector3Int(-9, -1, 0), null);
            doors[3].SetActive(true);
        }
    }

    public void OnRoomEnter()
    {
        hudManager.CenterMapOnPoint(this.transform.position);
        if (!room.isCleared)
        {
            map.SetActive(true);
            foreach (GameObject obj in spawners)
            {
                EnemySpawner spawner = obj.GetComponent<EnemySpawner>();
                spawner.room = room;
                Enemy spawned = spawner.GetComponent<EnemySpawner>().SpawnEnemies();
                if (spawned != null)
                {
                    spawned.OnDeath += OnEnemyDeath;
                    enemyCount++;
                }
            }
            if (enemyCount == 0)
            {
                room.isCleared = true;
                BuildDoors();
            }
        }
    }

    void CloseDoors()
    {
        if (room.isCleared || room != levelManager.currentRoom) {
            return;
        }

        if ((room.doors & 1 << 0) != 0)
        {
            tilemap.SetTile(new Vector3Int(0, 4, 0), tile);
            tilemap.SetTile(new Vector3Int(-1, 4, 0), tile);
        }
        if ((room.doors & 1 << 1) != 0)
        {
            tilemap.SetTile(new Vector3Int(0, -5, 0), tile);
            tilemap.SetTile(new Vector3Int(-1, -5, 0), tile);
        }
        if ((room.doors & 1 << 2) != 0)
        {
            tilemap.SetTile(new Vector3Int(8, 0, 0), tile);
            tilemap.SetTile(new Vector3Int(8, -1, 0), tile);
        }
        if ((room.doors & 1 << 3) != 0)
        {
            tilemap.SetTile(new Vector3Int(-9, 0, 0), tile);
            tilemap.SetTile(new Vector3Int(-9, -1, 0), tile);
        }
    }

    void OnEnemyDeath()
    {
        enemyCount--;
        if (enemyCount == 0)
        {
            room.isCleared = true;
            GameObject pickup = Resources.Load<GameObject>("Prefabs/Pickups/Circle");
            GameObject instance = Object.Instantiate(pickup, transform.position, Quaternion.identity);
            BuildDoors();
        }
    }
}
