using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class TypewriterSubtitle : MonoBehaviour
{
    [Header("字幕")]
    public TextMeshProUGUI subtitle;

    [TextArea(3, 8)]
    public string content;

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

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (subtitle != null)
            subtitle.text = "";
    }

    /// <summary>
    /// 播放 Inspector 中填写的字幕
    /// </summary>
    public void PlaySubtitle()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    /// <summary>
    /// 播放指定字幕
    /// </summary>
    public void PlaySubtitle(string text)
    {
        content = text;
        PlaySubtitle();
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

    IEnumerator TypeText()
    {
        if (subtitle == null)
            yield break;

        subtitle.text = "";

        if (delay > 0)
            yield return new WaitForSeconds(delay);

        foreach (char c in content)
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