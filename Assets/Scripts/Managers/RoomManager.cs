using UnityEngine;
using UnityEngine.Tilemaps;

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
                    if (room.type == RoomType.Boss)
                    {
                        HudManager hudManager = FindAnyObjectByType<HudManager>();
                        Debug.Log(hudManager);
                        spawned.onHealthChange += hudManager.bossHP.GetComponent<HealthBar>().UpdateHealthBar;
                        hudManager.ToggleBossHP();
                    }
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
        if (enemyCount <= 0)
        {
            switch (room.type)
            {
                case RoomType.Boss:
                    if (levelManager.floor < 3)
                    {
                        GameObject floorProgressor = Resources.Load<GameObject>("Prefabs/FloorProgressor");
                        Object.Instantiate(floorProgressor, transform.position, Quaternion.identity);
                        HudManager hudManager = FindAnyObjectByType<HudManager>();
                        hudManager.ToggleBossHP();
                    }
                    else
                    {
                        GameObject floorProgressor = Resources.Load<GameObject>("Prefabs/GameFinisher");
                        Object.Instantiate(floorProgressor, transform.position, Quaternion.identity);
                        HudManager hudManager = FindAnyObjectByType<HudManager>();
                        hudManager.ToggleBossHP();
                    }
                    break;

                case RoomType.Normal:
                    GameObject pickup = Resources.Load<GameObject>("Prefabs/Pickups/Health Pickup");
                    GameObject instance = Object.Instantiate(pickup, transform.position, Quaternion.identity);
                    break;
            }
            room.isCleared = true;
            
            BuildDoors();
        }
    }
}
