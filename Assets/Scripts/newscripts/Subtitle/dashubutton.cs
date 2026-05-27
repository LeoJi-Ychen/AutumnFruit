using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AlphaRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
{
    public Image image;
    [Range(0, 1)]
    public float alphaThreshold = 0.3f; // ⭐ 调这个（推荐 0.2~0.4）

    public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
    {
        if (image == null || image.sprite == null)
            return false;

        RectTransform rectTransform = image.rectTransform;

        Vector2 localPoint;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, sp, eventCamera, out localPoint))
            return false;

        Rect rect = rectTransform.rect;

        float x = Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
        float y = Mathf.InverseLerp(rect.yMin, rect.yMax, localPoint.y);

        Texture2D tex = image.sprite.texture;

        int texX = Mathf.FloorToInt(x * tex.width);
        int texY = Mathf.FloorToInt(y * tex.height);

        try
        {
            Color color = tex.GetPixel(texX, texY);
            return color.a >= alphaThreshold;
        }
        catch
        {
            return false;
        }
    }
}