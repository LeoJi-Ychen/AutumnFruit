using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class SubtitleLine
{
    [TextArea(2, 5)]
    public string text;
    public float duration = 2f;
    public float interval = 1f;
}

public class SubtitleUI : MonoBehaviour
{
    [Header("UI引用")]
    public RectTransform root;
    public TextMeshProUGUI subtitleText;
    public Image background;

    [Header("字幕内容")]
    public List<SubtitleLine> subtitles = new List<SubtitleLine>();

    [Header("位置设置")]
    public Vector2 anchoredPosition = new Vector2(0, 80);

    [Header("文字设置")]
    public int fontSize = 42;
    public Color textColor = Color.white;
    public TextAlignmentOptions alignment = TextAlignmentOptions.Center;

    [Header("背景设置")]
    public Color backgroundColor = new Color(0, 0, 0, 0.4f);
    public Vector2 backgroundPadding = new Vector2(80, 40);

    [Header("播放控制")]
    public bool playOnStart = false;

    private bool hasPlayed = false;
    private Coroutine playRoutine;

    void Awake()
    {
        ApplyUISettings();
    }

    void Start()
    {
        subtitleText.text = "";

        if (playOnStart)
        {
            PlaySubtitles();
        }
    }

    void ApplyUISettings()
    {
        if (root != null)
        {
            root.anchoredPosition = anchoredPosition;
        }

        if (subtitleText != null)
        {
            subtitleText.fontSize = fontSize;
            subtitleText.color = textColor;
            subtitleText.alignment = alignment;
        }

        if (background != null)
        {
            background.color = backgroundColor;
            background.gameObject.SetActive(false);
        }
    }

    public void PlaySubtitles()
    {
        if (hasPlayed) return;
        hasPlayed = true;

        if (playRoutine != null)
            StopCoroutine(playRoutine);

        playRoutine = StartCoroutine(PlayCoroutine());
    }

    IEnumerator PlayCoroutine()
    {
        subtitleText.text = "";

        foreach (var line in subtitles)
        {
            // 显示文字
            subtitleText.text = line.text;

            // 等TMP刷新
            yield return null;

            UpdateBackgroundSize();

            yield return new WaitForSeconds(line.duration);

            subtitleText.text = "";

            if (background != null)
                background.gameObject.SetActive(false);

            yield return new WaitForSeconds(line.interval);
        }

        subtitleText.text = "";
    }

    void UpdateBackgroundSize()
    {
        if (background == null || subtitleText == null) return;

        subtitleText.ForceMeshUpdate();

        Vector2 textSize = subtitleText.GetRenderedValues(false);

        background.gameObject.SetActive(true);

        // 设置背景大小 = 文字大小 + padding
        background.rectTransform.sizeDelta = textSize + backgroundPadding;

        background.color = backgroundColor;

        // 让背景跟随文字位置
        background.rectTransform.anchoredPosition = subtitleText.rectTransform.anchoredPosition;
    }
}