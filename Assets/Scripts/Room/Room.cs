using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Room
{
    public Vector2 gridPos;
    public RoomType type;
    public int doors = 0;

    public bool isCleared = false;

    public Room(Vector2 gridPos, RoomType type)
    {
        this.gridPos = gridPos;
        this.type = type;
        if (type == RoomType.Starting)
        {
            isCleared = true;
        }
    }
}
