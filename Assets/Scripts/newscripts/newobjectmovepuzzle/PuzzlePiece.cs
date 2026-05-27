using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class PuzzlePiece : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("功能开关")]
    public bool enableDrag = true;
    public bool enableHover = true;
    public bool enableClickScale = true;

    [Header("拼图目标")]
    public PuzzleTarget target;
    public float snapSpeed = 10f;

    [Header("⭐ 所属拼图管理器（关键）")]
    public PuzzleManager manager;

    [Header("缩放")]
    public float hoverScale = 1.05f;
    public float clickScale = 1.1f;
    public float scaleSpeed = 10f;

    [Header("层级控制")]
    public bool changeLayerOnSnap = true;
    public int layerOffset = 1;

    // ⭐⭐⭐ 新增（仅这一行）
    public System.Action<PuzzlePiece> onPlaced;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private bool isPointerOver = false;
    private bool isDragging = false;
    private bool isClicking = false;
    private bool isLocked = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;

        // 防止复用问题
        isLocked = false;
    }

    void Update()
    {
        // ⭐⭐⭐ 关键：完成后停止缩放系统
        if (isLocked) return;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    // 👉 Hover
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isLocked) return;

        isPointerOver = true;

        if (enableHover && !isClicking)
            targetScale = originalScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isLocked) return;

        isPointerOver = false;

        if (!isDragging)
            targetScale = originalScale;
    }

    // 👉 Click
    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return;

        isClicking = true;

        if (enableClickScale)
            targetScale = originalScale * clickScale;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked) return;

        isClicking = false;

        if (enableHover && isPointerOver)
            targetScale = originalScale * hoverScale;
        else
            targetScale = originalScale;
    }

    // 👉 Drag
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enableDrag || isLocked) return;

        isDragging = true;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!enableDrag || isLocked) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (target != null)
        {
            target.CheckHover(rectTransform.anchoredPosition);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!enableDrag || isLocked) return;

        isDragging = false;

        if (target != null && target.IsHovering())
        {
            StartCoroutine(SnapToTarget());
        }
        else
        {
            if (enableHover && isPointerOver)
                targetScale = originalScale * hoverScale;
            else
                targetScale = originalScale;
        }
    }

    // 👉 吸附 + 锁定
    IEnumerator SnapToTarget()
    {
        Vector2 start = rectTransform.anchoredPosition;
        Vector2 end = ((RectTransform)target.transform).anchoredPosition;

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * snapSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        rectTransform.anchoredPosition = end;

        // ⭐ 层级控制（保持不变）
        if (changeLayerOnSnap && target != null)
        {
            int targetIndex = target.transform.GetSiblingIndex();
            int newIndex = targetIndex + layerOffset;

            int max = transform.parent.childCount - 1;
            newIndex = Mathf.Clamp(newIndex, 0, max);

            transform.SetSiblingIndex(newIndex);
        }

        // ⭐⭐⭐ 锁定（关键）
        isLocked = true;

        // ⭐ 保持当前缩放（防止被改回）
        targetScale = transform.localScale;

        // 👉 关闭点击
        var img = GetComponent<UnityEngine.UI.Image>();
        if (img != null)
            img.raycastTarget = false;

        // 👉 通知目标
        target.OnPlaced(this);

        // 👉 通知管理器
        if (manager != null)
        {
            manager.NotifyPiecePlaced(this);
        }
        else
        {
            Debug.LogWarning("⚠️ 没绑定 PuzzleManager：" + name);
        }

        // ⭐⭐⭐ 新增（只这一行，不影响任何原逻辑）
        onPlaced?.Invoke(this);
    }
}