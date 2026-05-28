using UnityEngine;
using UnityEngine.UI;
public class WaterProgressBar : MonoBehaviour
{
    public float initWidth;
    public float progress;
    public GameObject bar;
    public RectTransform bar_line;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bar_line = bar.GetComponent<RectTransform>();
        initWidth = bar_line.rect.width;
        progress = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bar_line.sizeDelta = new Vector2((float)initWidth * progress, bar_line.sizeDelta.y);
    }
}
