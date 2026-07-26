using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class DialogueTypewriter : MonoBehaviour
{
    [System.Serializable]
    public class DialoguePage
    {
        [Header("显示位置（TextMeshProUGUI）")]
        public TextMeshProUGUI target;

        [Header("字幕内容")]
        [TextArea(3, 8)]
        public string content;
    }

    [Header("剧情字幕列表")]
    public List<DialoguePage> pages = new List<DialoguePage>();

    [Header("播放到最后是否循环")]
    public bool loop = false;

    [Header("打字速度")]
    public float characterInterval = 0.05f;

    [Header("开始延迟")]
    public float delay = 0f;

    [Header("播放完成后自动隐藏")]
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

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        ClearAllDialogue();
    }

    /// <summary>
    /// Button调用，每点击一次播放下一段
    /// </summary>
    public void PlayNextDialogue()
    {
        if (pages.Count == 0)
            return;

        if (currentIndex >= pages.Count)
        {
            if (loop)
                currentIndex = 0;
            else
                return;
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeDialogue(pages[currentIndex]));

        currentIndex++;
    }

    IEnumerator TypeDialogue(DialoguePage page)
    {
        if (page.target == null)
            yield break;

        page.target.text = "";

        if (delay > 0)
            yield return new WaitForSeconds(delay);

        foreach (char c in page.content)
        {
            page.target.text += c;

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
            page.target.text = "";
        }
    }

    /// <summary>
    /// 从第一句重新开始
    /// </summary>
    public void ResetDialogue()
    {
        currentIndex = 0;
    }

    /// <summary>
    /// 清空所有字幕
    /// </summary>
    public void ClearAllDialogue()
    {
        foreach (DialoguePage page in pages)
        {
            if (page.target != null)
                page.target.text = "";
        }
    }

    /// <summary>
    /// 跳到指定页（第一页是0）
    /// </summary>
    public void SetDialogueIndex(int index)
    {
        currentIndex = Mathf.Clamp(index, 0, pages.Count - 1);
    }

    /// <summary>
    /// 获取当前播放页
    /// </summary>
    public int GetCurrentIndex()
    {
        return currentIndex;
    }
}