using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorProgressor : MonoBehaviour
{
    LevelManager levelManager;
    bool isActive;
    SpriteRenderer spriteRenderer;

    public void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Color color = spriteRenderer.color;
        color.a = 0.25f;
        spriteRenderer.color = color;
        levelManager = FindAnyObjectByType<LevelManager>();
        StartCoroutine("Cooldown");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            levelManager.NextFloor();
        } else
        {
            StopCoroutine("Cooldown");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        StartCoroutine("Cooldown");
    }

    public IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(1);
        isActive = true;
        Color color = spriteRenderer.color;
        color.a = 1;
        spriteRenderer.color = color;
    }
}
