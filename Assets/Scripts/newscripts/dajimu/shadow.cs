using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDragShadow : MonoBehaviour
{
    [Header("偏移")]
    public Vector2 offset = new Vector2(8f, -8f);

    [Header("阴影颜色")]
    public Color shadowColor = new Color(0, 0, 0, 0.2f);

    private RectTransform rect;
    private RectTransform shadow;
    private Image shadowImage;
    private Image originalImage;

    private PuzzlePiece piece;

    private bool isInitialized = false;
    private bool isDisabled = false;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        originalImage = GetComponent<Image>();
        piece = GetComponent<PuzzlePiece>();
    }

    void Start()
    {
        StartCoroutine(Init());

        // ⭐⭐⭐ 监听拼图完成
        if (piece != null)
        {
            piece.onPlaced += OnPuzzlePlaced;
        }
    }

    IEnumerator Init()
    {
        yield return null; // 等UI初始化

        CreateShadow();

        isInitialized = true;
    }

    void CreateShadow()
    {
        GameObject obj = new GameObject(name + "_Shadow");
        obj.transform.SetParent(transform.parent);

        shadow = obj.AddComponent<RectTransform>();
        shadowImage = obj.AddComponent<Image>();

        if (originalImage != null)
        {
            shadowImage.sprite = originalImage.sprite;
            shadowImage.type = originalImage.type;
            shadowImage.preserveAspect = true;
        }

        shadowImage.raycastTarget = false;

        shadow.sizeDelta = rect.sizeDelta;
        shadow.localScale = rect.localScale;
        shadow.rotation = rect.rotation;
    }

    void LateUpdate()
    {
        if (!isInitialized || shadow == null || isDisabled) return;

        // ⭐ Appear 兼容（物体隐藏就关）
        if (!gameObject.activeInHierarchy)
        {
            shadow.gameObject.SetActive(false);
            return;
        }

        float baseAlpha = originalImage != null ? originalImage.color.a : 1f;

        // ⭐ 完全透明也隐藏
        if (baseAlpha <= 0.01f)
        {
            shadow.gameObject.SetActive(false);
            return;
        }

        shadow.gameObject.SetActive(true);

        // ⭐ 同步形状（不会变形）
        shadow.sizeDelta = rect.sizeDelta;
        shadow.localScale = rect.localScale;
        shadow.rotation = rect.rotation;

        // ⭐ 位置
        shadow.anchoredPosition = rect.anchoredPosition + offset;

        // ⭐ 透明度跟随
        Color c = shadowColor;
        c.a *= baseAlpha;
        shadowImage.color = c;

        // ⭐⭐⭐ 层级锁死（永远在下面）
        SyncLayer();
    }

    void SyncLayer()
    {
        if (shadow == null) return;

        int index = rect.GetSiblingIndex();
        shadow.SetSiblingIndex(Mathf.Max(0, index - 1));
    }

    // ⭐⭐⭐ 拼图完成时调用（核心）
    void OnPuzzlePlaced(PuzzlePiece p)
    {
        DisableShadow();
    }

    void DisableShadow()
    {
        isDisabled = true;

        if (shadow != null)
            shadow.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        // ⭐ 防止内存残留
        if (piece != null)
        {
            piece.onPlaced -= OnPuzzlePlaced;
        }
    }
}