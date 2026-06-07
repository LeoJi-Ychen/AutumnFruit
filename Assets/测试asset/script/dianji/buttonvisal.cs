using UnityEngine;
using UnityEngine.EventSystems;

public class UIButtonFX : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    [Header("开关")]
    public bool enableHover = true;
    public bool enableClick = true;

    [Header("缩放")]
    public float hoverScale = 1.05f;
    public float clickScale = 1.1f;
    public float scaleSpeed = 10f;

    [Header("粒子")]
    public ParticleSystem clickParticle;

    private Vector3 originalScale;
    private Vector3 targetScale;

    private bool isPointerOver = false;
    private bool isClicking = false;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(
            transform.localScale,
            targetScale,
            Time.deltaTime * scaleSpeed
        );
    }

    // 👉 鼠标进入
    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOver = true;

        if (enableHover && !isClicking)
            targetScale = originalScale * hoverScale;
    }

    // 👉 鼠标离开
    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOver = false;

        if (!isClicking)
            targetScale = originalScale;
    }

    // 👉 点击按下
    public void OnPointerDown(PointerEventData eventData)
    {
        isClicking = true;

        if (enableClick)
        {
            targetScale = originalScale * clickScale;
            PlayParticle();
        }
    }

    // 👉 点击松开
    public void OnPointerUp(PointerEventData eventData)
    {
        isClicking = false;

        if (enableHover && isPointerOver)
            targetScale = originalScale * hoverScale;
        else
            targetScale = originalScale;
    }

    void PlayParticle()
    {
        if (clickParticle == null) return;

        ParticleSystem p = Instantiate(
            clickParticle,
            transform.position,
            Quaternion.identity,
            transform
        );

        p.Play();
        Destroy(p.gameObject, 1.5f);
    }
}
