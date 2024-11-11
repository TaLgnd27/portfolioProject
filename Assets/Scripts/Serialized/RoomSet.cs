using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRoomSet", menuName = "Level/RoomSet")]
public class RoomSet : ScriptableObject
{
    [SerializeField]
    public GameObject[] rooms;
}
