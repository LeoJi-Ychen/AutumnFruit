using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SimpleAudioPlayer : MonoBehaviour
{
    public enum AudioType
    {
        BGM,
        Voice,
        SFX
    }

    [Header("类型")]
    public AudioType audioType;

    [Header("音频")]
    public AudioClip clip;

    [Header("播放设置")]
    public bool playOnStart = false;
    public bool loop = false;

    [Header("旁白只播放一次")]
    public bool playOnlyOnce = false;
    private bool hasPlayed = false;

    [Header("跨场景保留（BGM用）")]
    public bool dontDestroyOnLoad = false;

    [Header("播放完自动销毁（旁白用）")]
    public bool destroyAfterFinish = false;

    [Header("BGM淡出时间")]
    public float fadeTime = 1f;

    [Header("初始音量")]
    [Range(0f, 1f)]
    public float startVolume = 1f;

    private AudioSource audioSource;

    // ⭐ BGM唯一控制
    private static SimpleAudioPlayer currentBGM;
    private static SimpleAudioPlayer persistentBGM;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        ApplySettings();
        audioSource.volume = startVolume;

        // =========================
        // 🎵 跨场景BGM逻辑
        // =========================
        if (dontDestroyOnLoad)
        {
            if (audioType == AudioType.BGM)
            {
                if (persistentBGM != null && persistentBGM != this)
                {
                    Destroy(gameObject);
                    return;
                }

                persistentBGM = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    void Start()
    {
        if (playOnStart)
        {
            Play();
        }
    }

    void Update()
    {
        // ⭐ 旁白播放完自动销毁
        if (destroyAfterFinish &&
            audioType == AudioType.Voice &&
            hasPlayed &&
            !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    void ApplySettings()
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
    }

    // 🎵 主播放
    public void Play()
    {
        ApplySettings();

        // =========================
        // 🎵 BGM（互相替换 + 跨场景）
        // =========================
        if (audioType == AudioType.BGM)
        {
            if (currentBGM != null && currentBGM != this)
            {
                currentBGM.StartFadeOut();
            }

            currentBGM = this;

            audioSource.volume = startVolume;
            audioSource.Play();
            return;
        }

        // =========================
        // 🗣 Voice（旁白）
        // =========================
        if (audioType == AudioType.Voice)
        {
            if (playOnlyOnce && hasPlayed)
                return;

            audioSource.volume = startVolume;
            audioSource.Play();

            hasPlayed = true;
            return;
        }

        // =========================
        // 🔊 SFX
        // =========================
        audioSource.volume = startVolume;
        audioSource.Play();
    }

    public void TriggerPlay()
    {
        Play();
    }

    // 🌙 BGM淡出
    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float startVol = audioSource.volume;
        float t = 0;

        while (t < fadeTime)
        {
            t += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVol, 0, t / fadeTime);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }

    // 🔄 重置旁白
    public void ResetVoice()
    {
        hasPlayed = false;
    }
}