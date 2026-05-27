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
    public UnityEvent onCompleteEvent;   // 👈 Inspector 可绑定

    private int totalClouds;
    private int currentCount = 0;

    private float currentProgress = 0f;
    private float targetProgress = 0f;

    private bool isFinished = false;

    void Start()
    {
        CloudClick[] clouds = FindObjectsOfType<CloudClick>();
        totalClouds = clouds.Length;

        foreach (var cloud in clouds)
        {
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

        // ⭐ 触发事件
        onCompleteEvent?.Invoke();

        // 可选：自动下一关
        Invoke(nameof(LoadNextScene), 1f);
    }

    void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}