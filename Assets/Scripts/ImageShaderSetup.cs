using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageShaderSetup : MonoBehaviour
{
    [Header("基础设置")]
    [Range(0f, 5f)]
    public float brightness = 1f;

    [Range(0f, 2f)]
    public float contrast = 1f;

    [Range(0f, 2f)]
    public float saturation = 1f;

    [Header("发光设置")]
    public Color glowColor = new Color(1, 1, 0.5f, 1);
    [Range(0f, 3f)]
    public float glowIntensity = 0f;
    [Range(0f, 0.1f)]
    public float glowSize = 0.02f;

    private Image targetImage;
    private Material brightnessMaterial;

    private static readonly int BrightnessID = Shader.PropertyToID("_Brightness");
    private static readonly int ContrastID = Shader.PropertyToID("_Contrast");
    private static readonly int SaturationID = Shader.PropertyToID("_Saturation");
    private static readonly int GlowColorID = Shader.PropertyToID("_GlowColor");
    private static readonly int GlowIntensityID = Shader.PropertyToID("_GlowIntensity");
    private static readonly int GlowSizeID = Shader.PropertyToID("_GlowSize");

    void Start()
    {
        targetImage = GetComponent<Image>();
        SetupMaterial();
    }

    void SetupMaterial()
    {
        Shader shader = Shader.Find("UI/BrightnessGlow");
        if (shader == null)
        {
            Debug.LogError("找不到 Shader: UI/BrightnessGlow，请确保 Shader 文件在项目中");
            return;
        }

        brightnessMaterial = new Material(shader);
        targetImage.material = brightnessMaterial;
        UpdateShaderProperties();
    }

    void Update()
    {
        if (brightnessMaterial != null)
        {
            UpdateShaderProperties();
        }
    }

    void UpdateShaderProperties()
    {
        brightnessMaterial.SetFloat(BrightnessID, brightness);
        brightnessMaterial.SetFloat(ContrastID, contrast);
        brightnessMaterial.SetFloat(SaturationID, saturation);
        brightnessMaterial.SetColor(GlowColorID, glowColor);
        brightnessMaterial.SetFloat(GlowIntensityID, glowIntensity);
        brightnessMaterial.SetFloat(GlowSizeID, glowSize);
    }

    // 公共方法供外部调用
    public void SetBrightness(float value)
    {
        brightness = Mathf.Clamp(value, 0f, 5f);
    }

    public void SetContrast(float value)
    {
        contrast = Mathf.Clamp(value, 0f, 2f);
    }

    public void SetSaturation(float value)
    {
        saturation = Mathf.Clamp(value, 0f, 2f);
    }

    public void SetGlowIntensity(float value)
    {
        glowIntensity = Mathf.Clamp(value, 0f, 3f);
    }

    public void SetGlowColor(Color color)
    {
        glowColor = color;
    }

    void OnDestroy()
    {
        if (brightnessMaterial != null)
        {
            Destroy(brightnessMaterial);
        }
    }
}
