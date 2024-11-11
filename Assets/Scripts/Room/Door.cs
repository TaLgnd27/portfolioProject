using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    GameObject playerPoint;
    [SerializeField]
    RoomManager roomGenerator;

    LevelManager levelManager;

    private void Start()
    {
        levelManager = FindFirstObjectByType<LevelManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(roomGenerator.room != levelManager.currentRoom && collision.gameObject.CompareTag("Player"))
        {
            levelManager.currentRoom = roomGenerator.room;
            Camera.main.GetComponent<CameraController>().target = roomGenerator.transform.position;
            //Camera.main.transform.position = new Vector3(roomGenerator.transform.position.x, roomGenerator.transform.position.y, Camera.main.transform.position.z);
            //collision.gameObject.GetComponent<Rigidbody2D>().MovePosition(playerPoint.transform.position);
            collision.gameObject.GetComponent<Player>().isTransition = true;
            collision.gameObject.GetComponent<Player>().moveTarget = playerPoint.transform.position;
            roomGenerator.OnRoomEnter();
        }
    }
}
