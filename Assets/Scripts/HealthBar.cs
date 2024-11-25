using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image fillImage;

    private void Start()
    {
        fillImage.type = Image.Type.Filled;
    }

    public void UpdateHealthBar(float percent)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = percent;
        }
    }
}
