using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField]
    Image img;
    Color color;
    public float fadeDuration = 1.0f;
    public float fadeStart;
    public float fadeEnd;

    private void Awake()
    {
        color = img.color;
        StartCoroutine(FadeAlpha(fadeStart,fadeEnd));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private System.Collections.IEnumerator FadeAlpha(float startAlpha, float endAlpha)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // Update text color with new alpha
            Color newColor = new Color(color.r, color.g, color.b, alpha);
            img.color = newColor;

            yield return null;
        }

        // Ensure the final alpha is set
        Color finalColor = new Color(color.r, color.g, color.b, endAlpha);
        img.color = finalColor;
        Destroy(gameObject);
    }
}
