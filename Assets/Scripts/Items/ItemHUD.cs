using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemHUD : MonoBehaviour
{
    [SerializeField]
    Canvas itemHud;

    [SerializeField]
    GameObject itemName;



    // Start is called before the first frame update
    public void InitItem(Item item)
    {
        itemName.GetComponent<TextMeshProUGUI>().text = item.itemName;
    }

    public void InitGun(Gun gun)
    {
        itemName.GetComponent<TextMeshProUGUI>().text = gun.gunName;
    }

    // Update is called once per frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        itemHud.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        itemHud.enabled = false;
    }
}
