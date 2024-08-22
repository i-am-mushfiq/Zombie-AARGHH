using UnityEngine;
using UnityEngine.Rendering.Universal; // Make sure to import the Universal Rendering package

public class ImageOpacityController : MonoBehaviour
{
    public SpriteRenderer sprite1;
    public SpriteRenderer sprite2;

    public Light2D light;


    [SerializeField]
    private float duration = 10f; // Duration for opacity and light changes
    private bool increasing = true;
    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        float alpha;
        float lightValue;

        if (increasing)
        {
            // Increasing opacity
            alpha = Mathf.Lerp(0, 1, timer / duration);
            lightValue = Mathf.Lerp(80, 255, timer / duration);

            SetSpriteOpacity(alpha);
            SetLightColor(lightValue);

            if (timer >= duration)
            {
                increasing = false;
                timer = 0f; // Reset timer for the next phase
            }
        }
        else
        {
            // Decreasing opacity
            alpha = Mathf.Lerp(1, 0, timer / duration);
            lightValue = Mathf.Lerp(255, 80, timer / duration);

            SetSpriteOpacity(alpha);
            SetLightColor(lightValue);

            if (timer >= duration)
            {
                increasing = true;
                timer = 0f; // Reset timer for the next phase
            }
        }
    }

    private void SetSpriteOpacity(float alpha)
    {
        Color color1 = sprite1.color;
        color1.a = alpha;
        sprite1.color = color1;

        Color color2 = sprite2.color;
        color2.a = alpha;
        sprite2.color = color2;
    }

    private void SetLightColor(float value)
    {
        Color color1 = light.color;
        color1.r = value / 255f; // Normalize value to 0-1 range
        color1.g = value / 255f; // Normalize value to 0-1 range
        light.color = color1;
    }
}
