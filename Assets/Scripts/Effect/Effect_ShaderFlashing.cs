using UnityEngine;

public class Effect_ShaderFlashing : MonoBehaviour
{
    public ImageShaderSetup shaderSetup;
    float timer;

    [Header("亮度呼吸")]
    public bool enableChangeBrightness = false;
    public float brightnessSpeed = 2f;
    public float minBright = 1f;
    public float maxBright = 2f;

    [Header("发光呼吸（可选）")]
    public bool enableGlowBreathing = true;
    public float glowSpeed = 0.8f;
    public float minGlow = 0f;
    public float maxGlow = 0.6f;

    [Header("颜色渐变（可选）")]
    public bool enableColorShift = false;
    public Color colorA = Color.white;
    public Color colorB = new Color(1, 0.8f, 0.5f);

    [Header("颜色初始化")]
    public bool init = true;
    public Color InitColor = Color.white;

    private void Start()
    {
        shaderSetup = this.gameObject.AddComponent<ImageShaderSetup>();
        if (init)
        {
            shaderSetup.SetGlowColor(InitColor);
            shaderSetup.SetBrightness(1);
            shaderSetup.SetGlowIntensity(0);
        }
    }
    void Update()
    {
        timer += Time.deltaTime;
        // 亮度呼吸
        if (enableChangeBrightness)
        {
            float brightT = Mathf.PingPong(timer * brightnessSpeed, 1f);
            brightT = Mathf.SmoothStep(0f, 1f, brightT);
            float bright = Mathf.Lerp(minBright, maxBright, brightT);
            shaderSetup.SetBrightness(bright);
        }
        // 发光呼吸
        if (enableGlowBreathing)
        {
            float glowT = Mathf.PingPong(timer * glowSpeed, 1f);
            glowT = Mathf.SmoothStep(0f, 1f, glowT);
            float glow = Mathf.Lerp(minGlow, maxGlow, glowT);
            shaderSetup.SetGlowIntensity(glow);
        }
        // 颜色渐变
        if (enableColorShift)
        {
            float colorT = Mathf.PingPong(timer * brightnessSpeed * 0.5f, 1f);
            Color currentColor = Color.Lerp(colorA, colorB, colorT);
            shaderSetup.SetGlowColor(currentColor);
        }
    }
}
