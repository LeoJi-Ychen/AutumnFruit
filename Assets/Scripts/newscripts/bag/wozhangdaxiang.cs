using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIDragLine : MonoBehaviour,
    IPointerDownHandler,
    IPointerUpHandler,
    IDragHandler
{
    [Header("A点和B点")]
    public RectTransform pointA;
    public RectTransform pointB;

    [Header("拖动目标")]
    public RectTransform target;

    [Header("点击放大")]
    public float scaleOnClick = 1.1f;
    public float scaleSpeed = 10f;

    [Header("触发")]
    public float triggerThreshold = 0.95f;
    public UnityEvent onReachEnd;

    [Header("回弹速度")]
    public float returnSpeed = 10f;

    [Header("终点锁定")]
    public bool lockAfterReachEnd = true; // ⭐ 新增开关

    private bool isDragging = false;
    private bool isLocked = false; // ⭐ 是否已锁定

    private float t = 0f;
    private float dragOffsetT;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private bool isReturning = false;

    void Start()
    {
        originalScale = target.localScale;
        targetScale = originalScale;

        SetPositionByT();
    }

    void Update()
    {
        // 缩放动画
        target.localScale = Vector3.Lerp(target.localScale, targetScale, Time.deltaTime * scaleSpeed);

        // 回弹
        if (isReturning)
        {
            t = Mathf.Lerp(t, 0f, Time.deltaTime * returnSpeed);

            if (Mathf.Abs(t) < 0.001f)
            {
                t = 0f;
                isReturning = false;
            }

            SetPositionByT();
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isLocked) return; // ⭐ 已锁定直接不响应

        isDragging = true;
        isReturning = false;

        targetScale = originalScale * scaleOnClick;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            target.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        Vector2 A = pointA.anchoredPosition;
        Vector2 B = pointB.anchoredPosition;

        Vector2 AB = B - A;
        Vector2 AP = localPoint - A;

        float projection = Vector2.Dot(AP, AB.normalized);
        float length = AB.magnitude;

        float clickT = Mathf.Clamp01(projection / length);

        dragOffsetT = t - clickT;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isLocked) return;

        isDragging = false;
        targetScale = originalScale;

        if (t >= triggerThreshold)
        {
            t = 1f;
            SetPositionByT();

            if (onReachEnd != null)
                onReachEnd.Invoke();

            // ⭐ 到终点锁定
            if (lockAfterReachEnd)
            {
                isLocked = true;
            }
        }
        else
        {
            isReturning = true;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || isLocked) return;

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            target.parent as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        Vector2 A = pointA.anchoredPosition;
        Vector2 B = pointB.anchoredPosition;

        Vector2 AB = B - A;
        Vector2 AP = localPoint - A;

        float projection = Vector2.Dot(AP, AB.normalized);
        float length = AB.magnitude;

        float rawT = projection / length;

        t = Mathf.Clamp01(rawT + dragOffsetT);

        SetPositionByT();
    }

    void SetPositionByT()
    {
        target.anchoredPosition = Vector2.Lerp(
            pointA.anchoredPosition,
            pointB.anchoredPosition,
            t
        );
    }
}