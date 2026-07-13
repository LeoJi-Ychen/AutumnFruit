using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ImageAlphaHit : MonoBehaviour
{
    [Range(0f, 1f)]
    [Tooltip("透明度阈值，高于此值才响应点击")]
    public float alphaThreshold = 0.5f;

    void Start()
    {
        if (GetComponent<Image>())
        {
            Image image = GetComponent<Image>();

            // 关键：启用 Alpha Hit Test
            image.alphaHitTestMinimumThreshold = alphaThreshold;

            // 确保图片 Read/Write Enabled 已开启（见下方说明）
        }
    }
}
