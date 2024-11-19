using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapImageSelector : MonoBehaviour
{
    public Sprite spN, spS, spE, spW, spNS, spNE, spNW, spNSE, spNSW, spNEW, spSE, spSW, spSEW, spEW, spNSEW;

    public RoomType type;
    public int doors;

    public Color normalColor, enterColor, bossColor, itemColor, gunColor;

    Color mainColor;
    Image img;
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        mainColor = normalColor;
        PickSprite();
        PickColor();

        if (type == RoomType.Starting)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    void PickSprite()
    {
        switch(doors){
            case (int)DoorSide.N:
                img.sprite = spN;
                break;
            case (int)DoorSide.S:
                img.sprite = spS;
                break;
            case (int)DoorSide.E:
                img.sprite = spE;
                break;
            case (int)DoorSide.W:
                img.sprite = spW;
                break;
            case (int)DoorSide.NS:
                img.sprite = spNS;
                break;
            case (int)DoorSide.NE:
                img.sprite = spNE;
                break;
            case (int)DoorSide.NW:
                img.sprite = spNW;
                break;
            case (int)DoorSide.NSE:
                img.sprite = spNSE;
                break;
            case (int)DoorSide.NSW:
                img.sprite = spNSW;
                break;
            case (int)DoorSide.NEW:
                img.sprite = spNEW;
                break;
            case (int)DoorSide.SE:
                img.sprite = spSE;
                break;
            case (int)DoorSide.SW:
                img.sprite = spSW;
                break;
            case (int)DoorSide.SEW:
                img.sprite = spSEW;
                break;
            case (int)DoorSide.EW:
                img.sprite = spEW;
                break;
            case (int)DoorSide.NSEW:
                img.sprite = spNSEW;
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
        img.color = mainColor;
    }
}
