using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Image progressFill;
    public GameObject resultImage;

    [Header("Progress")]
    public float fillSpeed = 5f;

    [Header("完成触发 ⭐")]
    public UnityEvent onCompleteEvent;

    [Header("要统计的气泡 ⭐（手动拖）")]
    public CloudClick[] targetClouds;

    [Header("可选完成按钮 ⭐（未完成不可点击）")]
    public Button completeButton;   // 可以拖 Button 进来
    public UnityEvent onButtonClicked; // 按钮点击事件

    private int totalClouds;
    private int currentCount = 0;

    private float currentProgress = 0f;
    private float targetProgress = 0f;

    private bool isFinished = false;

    void Start()
    {
        // ✅ 只统计你拖进来的气泡
        totalClouds = targetClouds.Length;

        foreach (var cloud in targetClouds)
        {
            if (cloud != null)
                cloud.onCloudClicked += OnCloudClicked;
        }

        // 初始化进度条
        if (progressFill != null)
        {
            progressFill.type = Image.Type.Filled;
            progressFill.fillMethod = Image.FillMethod.Horizontal;
            progressFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            progressFill.fillAmount = 0f;
        }

        // 初始化结果面板
        if (resultImage != null)
            resultImage.SetActive(false);

        // 初始化按钮
        if (completeButton != null)
        {
            completeButton.interactable = false; // 初始不可点击
            completeButton.onClick.AddListener(() =>
            {
                if (isFinished)
                    onButtonClicked?.Invoke(); // 完成后才执行按钮事件
            });
        }
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

    void OnCloudClicked()
    {
        if (isFinished) return;

        currentCount++;
        targetProgress = (float)currentCount / totalClouds;

        // ⭐ 完成后触发
        if (currentCount >= totalClouds)
        {
            OnComplete();
        }
    }

    void OnComplete()
    {
        isFinished = true;

        // 显示结果
        if (resultImage != null)
            resultImage.SetActive(true);

        // 触发事件
        onCompleteEvent?.Invoke();

        // ✅ 按钮可点击
        if (completeButton != null)
            completeButton.interactable = true;

        Invoke(nameof(LoadNextScene), 1f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}