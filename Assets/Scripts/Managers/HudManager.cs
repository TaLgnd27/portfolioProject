using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HudManager : MonoBehaviour
{
    [SerializeField]
    public HealthBar healthBar;
    [SerializeField]
    public GameObject bossHP;
    [SerializeField]
    RectTransform map;
    [SerializeField]
    RectTransform mapContent;

    private Vector2 oldMapSize;
    private Vector2 oldMapPos;

    // Start is called before the first frame update
    void Start()
    {
        Player player = FindAnyObjectByType<Player>();
        if (player != null)
        {
            Debug.Log("Attaching hp listener");
            player.onHealthChange += UpdateHealth;
            player.RequestHealthUpdate();
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
        Vector2 newAnchoredPosition = targetWorldPosition * -5;

        // Clamp the position to avoid overscrolling
        //Vector2 clampedPosition = ClampToBounds(newAnchoredPosition);

        // Apply the new position
        mapContent.anchoredPosition = newAnchoredPosition;
    }

    public void OnMapInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            oldMapPos = mapContent.anchoredPosition;
            oldMapSize = map.sizeDelta;
            map.sizeDelta = oldMapSize * 2;
            Vector2 contentTargetScale = new Vector2(
            0.6f,
            0.6f);
            mapContent.localScale = contentTargetScale;
            CenterMapOnPoint(Vector2.zero);
        }
        else if (context.canceled)
        {
            map.sizeDelta = oldMapSize;
            mapContent.anchoredPosition = oldMapPos;
            mapContent.localScale = Vector2.one;
        }
    }
}
