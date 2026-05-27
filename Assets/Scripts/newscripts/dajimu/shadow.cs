using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIDragShadow : MonoBehaviour
{
    [Header("偏移")]
    public Vector2 offset = new Vector2(8f, -8f);

    [Header("阴影颜色")]
    public Color shadowColor = new Color(0, 0, 0, 0.2f);

    [Header("⭐Button控制")]
    public bool disableByButton = true;

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

        if (piece != null)
        {
            piece.onPlaced += OnPuzzlePlaced;
        }
    }

    IEnumerator Init()
    {
        yield return null;
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

        if (!gameObject.activeInHierarchy)
        {
            shadow.gameObject.SetActive(false);
            return;
        }

        float baseAlpha = originalImage != null ? originalImage.color.a : 1f;

        if (baseAlpha <= 0.01f)
        {
            shadow.gameObject.SetActive(false);
            return;
        }

        shadow.gameObject.SetActive(true);

        shadow.sizeDelta = rect.sizeDelta;
        shadow.localScale = rect.localScale;
        shadow.rotation = rect.rotation;

        shadow.anchoredPosition = rect.anchoredPosition + offset;

        Color c = shadowColor;
        c.a *= baseAlpha;
        shadowImage.color = c;

        SyncLayer();
    }

    void SyncLayer()
    {
        if (shadow == null) return;

        int index = rect.GetSiblingIndex();
        shadow.SetSiblingIndex(Mathf.Max(0, index - 1));
    }

    // ⭐⭐⭐ 拼图完成 → 关闭自己 + 所有子物体阴影
    void OnPuzzlePlaced(PuzzlePiece p)
    {
        UIDragShadow[] all = GetComponentsInChildren<UIDragShadow>(true);

        foreach (var s in all)
        {
            s.DisableShadow();
        }
    }

    // ⭐ Button调用
    public void DisableShadowByButton()
    {
        if (!disableByButton) return;

        UIDragShadow[] all = GetComponentsInChildren<UIDragShadow>(true);

        foreach (var s in all)
        {
            s.DisableShadow();
        }
    }

    void DisableShadow()
    {
        isDisabled = true;

        if (shadow != null)
            shadow.gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        if (piece != null)
        {
            piece.onPlaced -= OnPuzzlePlaced;
        }
    }
}