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

    [Header("⭐ 所属拼图管理器")]
    public PuzzleManager manager;

    [Header("缩放")]
    public float hoverScale = 1.05f;
    public float clickScale = 1.1f;
    public float scaleSpeed = 10f;

    [Header("层级控制")]
    public bool changeLayerOnSnap = true;
    public int layerOffset = 1;

    [Header("完成后设为子物体")]
    public bool setAsChildOnSnap = false;

    [Header("完成后删除")]
    public bool destroyAfterSnap = false;
    public float destroyDelay = 0f;

    public System.Action<PuzzlePiece> onPlaced;

    private RectTransform rectTransform;
    private Canvas canvas;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private bool isPointerOver = false;
    private bool isDragging = false;
    private bool isClicking = false;
    private bool isLocked = false;

    // 防止重复触发
    private bool hasSnapped = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        if (isLocked) return;

        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

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

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enableDrag || isLocked) return;

        isDragging = true;

        // ❌ 注释掉原来的层级提升
        // transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!enableDrag || isLocked) return;

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        if (target != null)
        {
            target.CheckHover(transform.position);
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
            targetScale = isPointerOver ? originalScale * hoverScale : originalScale;
        }
    }

    IEnumerator SnapToTarget()
    {
        if (hasSnapped) yield break;
        hasSnapped = true;

        Vector2 start = rectTransform.anchoredPosition;

        Vector2 end = rectTransform.parent.InverseTransformPoint(target.transform.position);

        float t = 0;

        while (t < 1)
        {
            t += Time.deltaTime * snapSpeed;
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);
            yield return null;
        }

        rectTransform.anchoredPosition = end;

        if (setAsChildOnSnap && target != null)
        {
            transform.SetParent(target.transform);
            rectTransform.anchoredPosition = Vector2.zero;
        }

        if (changeLayerOnSnap && target != null)
        {
            int targetIndex = target.transform.GetSiblingIndex();

            int newIndex = Mathf.Clamp(
                targetIndex + layerOffset,
                0,
                transform.parent.childCount - 1
            );

            transform.SetSiblingIndex(newIndex);
        }

        isLocked = true;
        targetScale = transform.localScale;

        var img = GetComponent<UnityEngine.UI.Image>();

        if (img != null)
            img.raycastTarget = false;

        if (target != null)
            target.OnPlaced(this);

        if (manager != null)
            manager.NotifyPiecePlaced(this);

        onPlaced?.Invoke(this);

        if (destroyAfterSnap)
        {
            yield return new WaitForSeconds(destroyDelay);
            Destroy(gameObject);
        }
    }
}