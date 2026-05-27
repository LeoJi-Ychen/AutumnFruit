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

    [Header("BGM淡出时间")]
    public float fadeTime = 1f;

    [Header("初始音量（关键🔥）")]
    [Range(0f, 1f)]
    public float startVolume = 1f;

    private AudioSource audioSource;

    // ⭐ 只控制BGM切换
    private static SimpleAudioPlayer currentBGM;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        ApplySettings();

        // ⭐ 只在初始化时设置一次音量
        audioSource.volume = startVolume;
    }

    void Start()
    {
        if (playOnStart)
        {
            Play();
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
        // 🎵 BGM（互相替换）
        // =========================
        if (audioType == AudioType.BGM)
        {
            if (currentBGM != null && currentBGM != this)
            {
                currentBGM.StartFadeOut();
            }

            currentBGM = this;

            audioSource.volume = startVolume; // ⭐ 不再强制=1
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