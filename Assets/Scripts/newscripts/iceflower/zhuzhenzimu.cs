using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class TypewriterSubtitle : MonoBehaviour
{
    [Header("字幕显示")]
    public TextMeshProUGUI subtitle;

    [Header("字幕列表（每点击一次播放下一句）")]
    [TextArea(3, 8)]
    public List<string> subtitleList = new List<string>();

    [Header("是否循环播放")]
    public bool loop = false;

    [Header("打字速度")]
    public float characterInterval = 0.05f;

    [Header("开始延迟")]
    public float delay = 0f;

    [Header("播放完成后隐藏")]
    public bool hideAfterFinish = false;

    [Header("隐藏延迟")]
    public float hideDelay = 1f;

    [Header("打字音效")]
    public AudioClip typingClip;

    [Range(0f, 1f)]
    public float typingVolume = 1f;

    [Header("播放完成事件")]
    public UnityEvent onFinished;

    private AudioSource audioSource;
    private Coroutine typingCoroutine;

    private int currentIndex = 0;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (subtitle != null)
            subtitle.text = "";
    }

    /// <summary>
    /// Button调用，每点击一次播放下一句
    /// </summary>
    public void PlaySubtitle()
    {
        if (subtitleList.Count == 0)
            return;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(subtitleList[currentIndex]));

        currentIndex++;

        if (loop)
        {
            if (currentIndex >= subtitleList.Count)
                currentIndex = 0;
        }
        else
        {
            currentIndex = Mathf.Min(currentIndex, subtitleList.Count - 1);
        }
    }

    /// <summary>
    /// 外部直接播放指定内容
    /// </summary>
    public void PlaySubtitle(string text)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    /// <summary>
    /// 重置回第一句
    /// </summary>
    public void ResetSubtitle()
    {
        currentIndex = 0;
    }

    /// <summary>
    /// 停止字幕
    /// </summary>
    public void StopSubtitle()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        subtitle.text = "";
    }

    IEnumerator TypeText(string text)
    {
        if (subtitle == null)
            yield break;

        subtitle.text = "";

        if (delay > 0)
            yield return new WaitForSeconds(delay);

        foreach (char c in text)
        {
            subtitle.text += c;

            if (typingClip != null && c != ' ')
            {
                audioSource.PlayOneShot(typingClip, typingVolume);
            }

            yield return new WaitForSeconds(characterInterval);
        }

        onFinished?.Invoke();

        if (hideAfterFinish)
        {
            yield return new WaitForSeconds(hideDelay);
            subtitle.text = "";
        }
    }
}