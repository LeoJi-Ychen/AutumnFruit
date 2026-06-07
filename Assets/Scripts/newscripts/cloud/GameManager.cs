using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    [Header("UI")]
    public Image progressFill;
    public GameObject resultImage;

    [Header("Progress")]
    public float fillSpeed = 5f;

    [Header("完成触发")]
    public UnityEvent onCompleteEvent;

    [Header("要统计的气泡（手动拖）")]
    public CloudClick[] targetClouds;

    [Header("可选完成按钮（未完成不可点击）")]
    public Button completeButton;
    public UnityEvent onButtonClicked;

    private int totalClouds;
    private int currentCount = 0;

    private float currentProgress = 0f;
    private float targetProgress = 0f;

    private bool isFinished = false;

    void Start()
    {
        totalClouds = targetClouds.Length;

        foreach (var cloud in targetClouds)
        {
            if (cloud != null)
                cloud.onCloudClicked += OnCloudClicked;
        }

        if (progressFill != null)
        {
            progressFill.type = Image.Type.Filled;
            progressFill.fillMethod = Image.FillMethod.Horizontal;
            progressFill.fillOrigin = (int)Image.OriginHorizontal.Left;
            progressFill.fillAmount = 0f;
        }

        if (resultImage != null)
            resultImage.SetActive(false);

        if (completeButton != null)
        {
            completeButton.interactable = false;

            completeButton.onClick.AddListener(() =>
            {
                if (isFinished)
                {
                    onButtonClicked?.Invoke();
                }
            });
        }
    }

    void Update()
    {
        if (progressFill != null)
        {
            currentProgress = Mathf.Lerp(
                currentProgress,
                targetProgress,
                Time.deltaTime * fillSpeed);

            if (Mathf.Abs(currentProgress - targetProgress) < 0.001f)
                currentProgress = targetProgress;

            progressFill.fillAmount = currentProgress;
        }
    }

    void OnCloudClicked()
    {
        if (isFinished)
            return;

        currentCount++;

        targetProgress =
            (float)currentCount / totalClouds;

        if (currentCount >= totalClouds)
        {
            OnComplete();
        }
    }

    void OnComplete()
    {
        isFinished = true;

        if (resultImage != null)
            resultImage.SetActive(true);

        onCompleteEvent?.Invoke();

        if (completeButton != null)
            completeButton.interactable = true;

        Debug.Log("气泡玩法完成");
    }
}