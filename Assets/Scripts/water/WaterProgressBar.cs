using UnityEngine;
using UnityEngine.UI;
public class WaterProgressBar : MonoBehaviour
{
    public float initWidth;
    public float progress;
    RectTransform bar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar = this.GetComponent<RectTransform>();
        initWidth = bar.rect.width;
        progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bar.sizeDelta = new Vector2((float)initWidth * progress,bar.sizeDelta.y);
    }
}
