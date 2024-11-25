using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

public class FloorGenerator : MonoBehaviour
{
    Vector2 worldSize = new Vector2(4, 4);

    Room[,] rooms;
    List<Vector2Int> takenPositions = new List<Vector2Int>();
    int gridSizeX, gridSizeY, numberOfRooms = 20;
    public GameObject mapBase;
    public GameObject roomBase;
    public RoomSet StandardRoomLayouts;
    public RoomSet StartingRoomLayouts;
    public RoomSet BossRoomLayouts;
    public RoomSet ItemRoomLayouts;
    public RoomSet GunRoomLayouts;

    List<Vector2Int> endrooms = new List<Vector2Int>();
    List<Vector2Int> latePositions = new List<Vector2Int>();
    [SerializeField]
    GameObject map;

    // Start is called before the first frame update
    void Start()
    {
        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        {
            numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
        }
        gridSizeX = Mathf.RoundToInt(worldSize.x);
        gridSizeY = Mathf.RoundToInt(worldSize.y);

        StandardRoomLayouts = Resources.Load<RoomSet>("Lists/StandardRooms");
        StartingRoomLayouts = Resources.Load<RoomSet>("Lists/StartingRooms");
        BossRoomLayouts = Resources.Load<RoomSet>("Lists/BossRooms");
        ItemRoomLayouts = Resources.Load<RoomSet>("Lists/ItemRooms");
        GunRoomLayouts = Resources.Load<RoomSet>("Lists/GunRooms");


        CreateRooms();
        SetRoomDoors();

        AddSpecialRooms();
        SetSpecialDoors();

        DrawMap();
    }

    void CreateRooms()
    {
        rooms = new Room[gridSizeX * 2, gridSizeY * 2];
        rooms[gridSizeX, gridSizeY] = new Room(Vector2.zero, RoomType.Starting);
        takenPositions.Insert(0, Vector2Int.zero);
        Vector2Int checkPos = Vector2Int.zero;

        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        for (int i = 0; i < numberOfRooms - 1; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);

            checkPos = NewPosition();

            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare)
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                if (iterations >= 50)
                {
                    print("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
                }
            }
            rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, RoomType.Normal);
            takenPositions.Insert(0, checkPos);
        }
    }

    Vector2Int NewPosition()
    {
        int x = 0, y = 0;
        Vector2Int checkingPos = Vector2Int.zero;
        do
        {
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2Int(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        return checkingPos;
    }

    Vector2Int SelectiveNewPosition()
    {
        int index = 0, inc = 0;
        int x = 0, y = 0;
        Vector2Int checkingPos = Vector2Int.zero;
        do
        {
            inc = 0;
            do
            {
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);
            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2Int(x, y);
        } while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);
        if (inc >= 100)
        {
            print("error: could not find a position with only than 1 neighbor");
        }
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2Int checkingPos, List<Vector2Int> usedPositions)
    {
        int ret = 0;
        if (usedPositions.Contains(checkingPos + Vector2Int.right))
        {
            ret++;
        }

        if (usedPositions.Contains(checkingPos + Vector2Int.left))
        {
            ret++;
        }

        if (usedPositions.Contains(checkingPos + Vector2Int.up))
        {
            ret++;
        }

        if (usedPositions.Contains(checkingPos + Vector2Int.down))
        {
            ret++;
        }

        return ret;
    }

    void SetRoomDoors()
    {
        for (int x = 0; x < gridSizeX * 2; x++)
        {
            for (int y = 0; y < gridSizeY * 2; y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }
                Vector2 gridPosition = new Vector2(x, y);
                if (y - 1 >= 0 && rooms[x, y - 1] != null)
                {
                    rooms[x, y].doors += (int)DoorSide.S;
                }
                if (y + 1 < gridSizeY * 2 && rooms[x, y + 1] != null)
                {
                    rooms[x, y].doors += (int)DoorSide.N;
                }
                if (x - 1 >= 0 && rooms[x - 1, y] != null)
                {
                    rooms[x, y].doors += (int)DoorSide.W;
                }
                if (x + 1 < gridSizeX * 2 && rooms[x + 1, y] != null)
                {
                    rooms[x, y].doors += (int)DoorSide.E;
                }

                if ((rooms[x, y].doors == 1 || rooms[x, y].doors == 2 || rooms[x, y].doors == 4 || rooms[x, y].doors == 8) && rooms[x, y].type != RoomType.Starting)
                {
                    endrooms.Add(new Vector2Int(x, y));
                    //rooms[x, y].type = (int)RoomType.Boss;
                }
            }
        }
    }

    void AddSpecialRooms()
    {
        rooms[endrooms[0].x, endrooms[0].y].type = RoomType.Boss;
        endrooms.Remove(endrooms[0]);
        foreach (RoomType type in System.Enum.GetValues(typeof(RoomType)))
        {    
            if (type != RoomType.Normal && type != RoomType.Starting && type != RoomType.Boss)
            {
                if (endrooms.Count > 1 && Random.value <= 0.5)
                {
                    rooms[endrooms[0].x, endrooms[0].y].type = type;
                    endrooms.Remove(endrooms[0]);
                }
                else
                {
                    Vector2Int checkPos = NewPosition();
                    int itr = 0;
                    while (!SpecialNeighbors(checkPos) && itr <= 100)
                    {
                        checkPos = NewPosition();
                        itr++;
                    }
                    if (!SpecialNeighbors(checkPos) || endrooms.Count <= 0)
                    {
                        rooms[checkPos.x + gridSizeX, checkPos.y + gridSizeY] = new Room(checkPos, type);
                        takenPositions.Add(checkPos);
                        latePositions.Add(checkPos);
                    }
                    else
                    {
                        //Debug.Log(rooms.ToString());
                        rooms[endrooms[0].x, endrooms[0].y].type = type;
                        endrooms.Remove(endrooms[0]);
                    }
                }
            }
        }
    }

    void SetSpecialDoors()
    {
        foreach (Vector2Int room in latePositions)
        {
            List<int> doors = new List<int> { 1, 2, 4, 8 };
            while (doors.Count > 0 && rooms[room.x + gridSizeX, room.y + gridSizeY].doors == 0)
            {
                int doorIndex = Random.Range(0, doors.Count - 1);
                int door = doors[doorIndex];
                doors.Remove(door);
                //Debug.Log(room);
                switch (door)
                {
                    case 1:
                        if (room.y + gridSizeY + 1 < gridSizeY * 2 && rooms[room.x + gridSizeX, room.y + gridSizeY + 1] != null && rooms[room.x + gridSizeX, room.y + gridSizeY + 1].type == RoomType.Normal)
                        {
                            rooms[room.x + gridSizeX, room.y + gridSizeY].doors += door;
                            rooms[room.x + gridSizeX, room.y + gridSizeY + 1].doors += 2;
                        }
                        break;
                    case 2:
                        if (room.y + gridSizeY - 1 >= 0 && rooms[room.x + gridSizeX, room.y + gridSizeY - 1] != null && rooms[room.x + gridSizeX, room.y + gridSizeY - 1].type == RoomType.Normal)
                        {
                            rooms[room.x + gridSizeX, room.y + gridSizeY].doors += door;
                            rooms[room.x + gridSizeX, room.y + gridSizeY - 1].doors += 1;
                        }
                        break;
                    case 4:
                        if (room.x + gridSizeX + 1 < gridSizeX * 2 && rooms[room.x + gridSizeX + 1, room.y + gridSizeY] != null && rooms[room.x + gridSizeX + 1, room.y + gridSizeY].type == RoomType.Normal)
                        {
                            rooms[room.x + gridSizeX, room.y + gridSizeY].doors += door;
                            rooms[room.x + gridSizeX + 1, room.y + gridSizeY].doors += 8;
                        }
                        break;
                    case 8:
                        if (room.x + gridSizeX - 1 >= 0 && rooms[room.x + gridSizeX - 1, room.y + gridSizeY] != null && rooms[room.x + gridSizeX - 1, room.y + gridSizeY].type == RoomType.Normal)
                        {
                            rooms[room.x + gridSizeX, room.y + gridSizeY].doors += door;
                            rooms[room.x + gridSizeX - 1, room.y + gridSizeY].doors += 4;
                        }
                        break;

                }
            }
        }

    }

    bool SpecialNeighbors(Vector2Int checkPos)
    {
        if ((checkPos.y + gridSizeY + 1 < gridSizeY*2 && (rooms[checkPos.x + gridSizeX, checkPos.y + gridSizeY + 1] != null && rooms[checkPos.x + gridSizeX, checkPos.y + gridSizeY + 1].type != RoomType.Normal))
            || (checkPos.y + gridSizeY - 1 >= 0 && (rooms[checkPos.x + gridSizeX, checkPos.y + gridSizeY - 1] != null && rooms[checkPos.x + gridSizeX, checkPos.y + gridSizeY - 1].type != RoomType.Normal))
            || (checkPos.x + gridSizeX + 1 < gridSizeX*2 && (rooms[checkPos.x + gridSizeX + 1, checkPos.y + gridSizeY] != null && rooms[checkPos.x + gridSizeX + 1, checkPos.y + gridSizeY].type != RoomType.Normal))
            || (checkPos.x + gridSizeX - 1 >= 0 && (rooms[checkPos.x + gridSizeX - 1, checkPos.y + gridSizeY] != null && rooms[checkPos.x + gridSizeX - 1, checkPos.y + gridSizeY].type != RoomType.Normal)))
        {
            return true;
        }
        return false;
    }

    void DrawMap()
    {
        foreach (Room room in rooms)
        {
            if (room == null)
            {
                continue;
            }
            Vector3 drawPos = (Vector3) room.gridPos;
            drawPos.x *= 18;
            drawPos.y *= 10;
            //drawPos.z += 20;
            GameObject mapper = Object.Instantiate(mapBase, parent: map.transform);
            mapper.transform.localPosition = drawPos * mapper.transform.localScale.x;
            MapImageSelector mapSelector = mapper.GetComponent<MapImageSelector>();
            mapSelector.type = room.type;
            mapSelector.doors = room.doors;

            GameObject layout = StartingRoomLayouts.rooms[Random.Range(0, StartingRoomLayouts.rooms.Length)];
            switch (room.type)
            {
                case RoomType.Normal:
                    layout = StandardRoomLayouts.rooms[Random.Range(0, StandardRoomLayouts.rooms.Length)];
                    break;
                case RoomType.Starting:
                    layout = StartingRoomLayouts.rooms[Random.Range(0, StartingRoomLayouts.rooms.Length)];
                    FindFirstObjectByType<LevelManager>().currentRoom = room;
                    break;
                case RoomType.Item:
                    layout = ItemRoomLayouts.rooms[Random.Range(0, ItemRoomLayouts.rooms.Length)];
                    break;
                case RoomType.Gun:
                    layout = GunRoomLayouts.rooms[Random.Range(0, GunRoomLayouts.rooms.Length)];
                    break;
                case RoomType.Boss:
                    layout = BossRoomLayouts.rooms[Random.Range(0, BossRoomLayouts.rooms.Length)];
                    break;
            }
            drawPos.z = 0;
            RoomManager generator = Object.Instantiate(layout, drawPos, Quaternion.identity).GetComponent<RoomManager>();
            generator.map = mapSelector.gameObject;
            generator.room = room;
        }
    }
}
