using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PuzzleTarget : MonoBehaviour
{
    [Header("检测")]
    public float hoverDistance = 100f;

    private bool isHovering = false;
    private bool isPlaced = false;

    [Header("事件")]
    public UnityEvent onPlaced;

    // ⭐⭐⭐ 新增：缩小替换模式（完全独立，不影响原逻辑）
    [Header("🔄 可选：缩小嵌入替换")]
    public bool useReplaceMode = false;
    public RectTransform replacePoint;     // 黑色区域
    public Vector3 replaceScale = Vector3.one;
    public float replaceSpeed = 8f;

    // 👉 检测是否进入范围
    public void CheckHover(Vector2 piecePos)
    {
        if (isPlaced) return;

        float dist = Vector2.Distance(
            ((RectTransform)transform).anchoredPosition,
            piecePos
        );

        isHovering = dist < hoverDistance;
    }

    public bool IsHovering()
    {
        return isHovering;
    }

    // 👉 拼图放上来
    public void OnPlaced(PuzzlePiece piece)
    {
        if (isPlaced) return;

        isPlaced = true;

        Debug.Log("🎯 拼图放置成功：" + name);

        // 👉 原有事件（不要删）
        onPlaced?.Invoke();

        // ⭐⭐⭐ 新增：缩小嵌入模式（完全额外逻辑）
        if (useReplaceMode && replacePoint != null)
        {
            piece.StartCoroutine(ReplaceToTarget(piece));
        }
    }

    // ⭐⭐⭐ 新增：缩小 + 移动 + 嵌入
    IEnumerator ReplaceToTarget(PuzzlePiece piece)
    {
        RectTransform pieceRect = piece.GetComponent<RectTransform>();

        Vector2 startPos = pieceRect.anchoredPosition;
        Vector2 endPos = replacePoint.anchoredPosition;

        Vector3 startScale = piece.transform.localScale;
        Vector3 endScale = replaceScale;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * replaceSpeed;

            pieceRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);
            piece.transform.localScale = Vector3.Lerp(startScale, endScale, t);

            yield return null;
        }

        pieceRect.anchoredPosition = endPos;
        piece.transform.localScale = endScale;

        // 👉 设为目标子物体（视觉嵌入）
        piece.transform.SetParent(replacePoint);

        // 👉 层级（保证显示正确）
        piece.transform.SetAsLastSibling();
    }
}