using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudManager : MonoBehaviour
{
    [SerializeField]
    HealthBar healthBar;
    [SerializeField]
    public GameObject bossHP;
    [SerializeField]
    RectTransform map;

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

    public void CenterMapOnPoint(Vector2 targetWorldPosition)
    {
        // Convert the world position to the local position relative to the map content
        //Vector2 localPosition = map.InverseTransformPoint(targetWorldPosition);

        // Calculate the new anchored position of the map content
        Vector2 newAnchoredPosition = targetWorldPosition*-5;

        // Clamp the position to avoid overscrolling
        //Vector2 clampedPosition = ClampToBounds(newAnchoredPosition);

        // Apply the new position
        map.anchoredPosition = newAnchoredPosition;
    }
}
