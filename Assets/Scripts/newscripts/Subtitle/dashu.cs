using UnityEngine;
using UnityEngine.UI;

public class TreeEnergyFinalAll : MonoBehaviour
{
    [Header("UI")]
    public Image progressFill;      
    public GameObject resultImage;

    [Header("Particle（生成用）⭐")]
    public ParticleSystem leafParticlePrefab; // 拖“预制体”！

    [Header("Button Image（可选）")]
    public Image treeImage;
    public Sprite normalSprite;
    public Sprite clickSprite;
    public Sprite finishedSprite;

    [Header("Progress")]
    public int totalClicksToFull = 10;

    [Header("Smooth")]
    public float fillSpeed = 5f;

    [Header("Direction")]
    public FillDirection fillDirection = FillDirection.LeftToRight;

    private int currentClicks = 0;
    private float currentProgress = 0f;
    private float targetProgress = 0f;

    private Button btn;
    private bool isFinished = false;

    public enum FillDirection
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    void Start()
    {
        btn = GetComponent<Button>();

        if (btn != null)
            btn.onClick.AddListener(OnClickTree);

        if (progressFill != null)
        {
            progressFill.type = Image.Type.Filled;
            SetFillDirection();
            progressFill.fillAmount = 0f;
        }

        if (treeImage != null && normalSprite != null)
        {
            treeImage.sprite = normalSprite;
        }

        if (resultImage != null)
            resultImage.SetActive(false);
    }

    void Update()
    {
        if (progressFill != null)
        {
            currentProgress = Mathf.Lerp(currentProgress, targetProgress, Time.deltaTime * fillSpeed);

            if (Mathf.Abs(currentProgress - targetProgress) < 0.001f)
                currentProgress = targetProgress;

            progressFill.fillAmount = currentProgress;
        }
    }

    void OnClickTree()
    {
        if (isFinished) return;

        // ⭐⭐⭐ 核心：生成新的粒子（不会影响旧的）
        if (leafParticlePrefab != null)
        {
            ParticleSystem newLeaf = Instantiate(
                leafParticlePrefab,
                transform.position,
                Quaternion.identity,
                transform   // 放在按钮下面（跟着UI）
            );

            newLeaf.Play();

            // ⭐ 自动销毁（防止堆积）
            Destroy(newLeaf.gameObject, newLeaf.main.duration + newLeaf.main.startLifetime.constantMax);
        }

        currentClicks++;
        targetProgress = (float)currentClicks / totalClicksToFull;

        if (treeImage != null && clickSprite != null)
        {
            treeImage.sprite = clickSprite;
        }

        if (currentClicks >= totalClicksToFull)
        {
            OnFull();
        }
    }

    void OnFull()
    {
        isFinished = true;

        if (resultImage != null)
            resultImage.SetActive(true);

        if (treeImage != null && finishedSprite != null)
        {
            treeImage.sprite = finishedSprite;
        }

        if (btn != null)
        {
            btn.enabled = false;
        }
    }

    void SetFillDirection()
    {
        switch (fillDirection)
        {
            case FillDirection.LeftToRight:
                progressFill.fillMethod = Image.FillMethod.Horizontal;
                progressFill.fillOrigin = (int)Image.OriginHorizontal.Left;
                break;

            case FillDirection.RightToLeft:
                progressFill.fillMethod = Image.FillMethod.Horizontal;
                progressFill.fillOrigin = (int)Image.OriginHorizontal.Right;
                break;

            case FillDirection.BottomToTop:
                progressFill.fillMethod = Image.FillMethod.Vertical;
                progressFill.fillOrigin = (int)Image.OriginVertical.Bottom;
                break;

            case FillDirection.TopToBottom:
                progressFill.fillMethod = Image.FillMethod.Vertical;
                progressFill.fillOrigin = (int)Image.OriginVertical.Top;
                break;
        }
    }
}