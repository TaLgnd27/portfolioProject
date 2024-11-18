using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    public GameObject bossHP;

    // Start is called before the first frame update
    void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        if(player != null)
        {
            //Debug.Log("Attaching hp listener");
            player.onHealthChange += UpdateHealth;
        }
    }
    
    public void UpdateHealth(float percent)
    {
        //Debug.Log(percent);
        healthBar.UpdateHealthBar(percent);
    }

    public void ToggleBossHP()
    {
        bossHP.SetActive(!bossHP.activeSelf);
    }
}
