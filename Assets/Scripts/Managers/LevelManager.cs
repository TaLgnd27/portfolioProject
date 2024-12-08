using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public Room currentRoom;
    public GameManager gameManager;
    public int floor;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        if ( gameManager != null)
        {
            floor = gameManager.floor;
        }
    }

    public void NextFloor()
    {
        gameManager.floor++;
        if (floor < 2)
        {
            gameManager.LoadNewFloor();
        } else
        {
            gameManager.EndGame();
        }
    }
}
