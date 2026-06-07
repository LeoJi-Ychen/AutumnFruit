using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class HealingInteractiveItem : MonoBehaviour,
    IPointerDownHandler,
    IBeginDragHandler,
    IDragHandler,
    IEndDragHandler
{
    [Header("Visual")]
    public Image targetImage;
    public Sprite[] stateSprites;

    [Header("Particle")]
    public ParticleSystem particlePrefab;

    [Header("Scale")]
    public float scaleMultiplier = 1.1f;   // 拖拽放大倍数
    public float clickScaleMultiplier = 1.1f; // 点击放大倍数
    public float scaleDuration = 0.15f;    // 动画时间

    private RectTransform rectTransform;
    private Canvas canvas;

    private int currentState = 0;
    private Vector3 originalScale;

    private bool isDragging = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void Start()
    {
        originalScale = transform.localScale;
    }

    // 👉 点击
    public void OnPointerDown(PointerEventData eventData)
    {
        PlayEffect();
        SwitchSprite();

        // 👉 如果不是拖拽才播放点击动画
        if (!isDragging)
        {
            StopAllCoroutines();
            StartCoroutine(ClickScale());
        }
    }

    // 👉 开始拖拽
    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;

        StopAllCoroutines(); // 防止点击动画干扰
        transform.localScale = originalScale * scaleMultiplier;
    }

    // 👉 拖拽中
    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // 👉 结束拖拽
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        transform.localScale = originalScale;
    }

    // 👉 点击缩放动画
    IEnumerator ClickScale()
    {
        Vector3 target = originalScale * clickScaleMultiplier;

        float time = 0;

        // 放大
        while (time < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(originalScale, target, time / scaleDuration);
            time += Time.deltaTime;
            yield return null;
        }

        // 缩回
        time = 0;
        while (time < scaleDuration)
        {
            transform.localScale = Vector3.Lerp(target, originalScale, time / scaleDuration);
            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = originalScale;
    }

    void PlayEffect()
    {
        if (particlePrefab != null)
        {
            ParticleSystem p = Instantiate(particlePrefab, transform.position, Quaternion.identity, transform);
            p.Play();
            Destroy(p.gameObject, 1.5f);
        }
    }

    void SwitchSprite()
    {
        if (stateSprites.Length == 0) return;

        currentState = (currentState + 1) % stateSprites.Length;
        targetImage.sprite = stateSprites[currentState];
    }

    public void ResetItem()
    {
        currentState = 0;
        if (stateSprites.Length > 0)
            targetImage.sprite = stateSprites[0];

        transform.localScale = originalScale;
    }
}