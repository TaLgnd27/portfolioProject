using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour
{
    public Sprite spN, spS, spE, spW, spNS, spNE, spNW, spNSE, spNSW, spNEW, spSE, spSW, spSEW, spEW, spNSEW;

    public RoomType type;
    public int doors;

    public Color normalColor, enterColor, bossColor, itemColor, gunColor;

    Color mainColor;
    SpriteRenderer rend;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        mainColor = normalColor;
        PickSprite();
        PickColor();
    }

    void PickSprite()
    {
        switch(doors){
            case (int)DoorSide.N:
                rend.sprite = spN;
                break;
            case (int)DoorSide.S:
                rend.sprite = spS;
                break;
            case (int)DoorSide.E:
                rend.sprite = spE;
                break;
            case (int)DoorSide.W:
                rend.sprite = spW;
                break;
            case (int)DoorSide.NS:
                rend.sprite = spNS;
                break;
            case (int)DoorSide.NE:
                rend.sprite = spNE;
                break;
            case (int)DoorSide.NW:
                rend.sprite = spNW;
                break;
            case (int)DoorSide.NSE:
                rend.sprite = spNSE;
                break;
            case (int)DoorSide.NSW:
                rend.sprite = spNSW;
                break;
            case (int)DoorSide.NEW:
                rend.sprite = spNEW;
                break;
            case (int)DoorSide.SE:
                rend.sprite = spSE;
                break;
            case (int)DoorSide.SW:
                rend.sprite = spSW;
                break;
            case (int)DoorSide.SEW:
                rend.sprite = spSEW;
                break;
            case (int)DoorSide.EW:
                rend.sprite = spEW;
                break;
            case (int)DoorSide.NSEW:
                rend.sprite = spNSEW;
                break;
        }
    }

    void PickColor()
    {
        switch (type)
        {
            case RoomType.Normal:
                mainColor = normalColor;
                break;
            case RoomType.Starting:
                mainColor = enterColor;
                break;
            case RoomType.Boss:
                mainColor = bossColor;
                break;
            case RoomType.Item:
                mainColor = itemColor;
                break;
            case RoomType.Gun:
                mainColor = gunColor;
                break;
        }
        rend.color = mainColor;
    }
}
