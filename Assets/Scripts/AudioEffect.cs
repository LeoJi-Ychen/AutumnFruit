using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioEffect : MonoBehaviour
{
    public int catalog;
    public bool loop_state;
    //public bool process_state;
    public bool init_state;
    public int init_id;
    IUF uf = new UIFunctions();
    public AudioSource audioSource;  // 在Inspector中拖拽赋值
    public List<AudioClip> soundClips;      // 音效文件
    public List<float> process;
    public int audioID;
    
    void Start()
    {
        string route = "AudioClips/"+catalog;
        audioSource = GetComponent<AudioSource>();
        uf.LoadAllResources<AudioClip>(route,soundClips);
        process = new List<float>(new float[soundClips.Count]);
        // 方式1：提前在Inspector中设置好AudioSource，然后代码控制循环
        if(soundClips.Count > 0 )
        {
            audioID = init_id;
            audioSource.clip = soundClips[audioID];
        }
        // 设置为循环播放
        //loop_state = true;
        audioSource.loop = loop_state;
        if (init_state)
        {
            audioSource.Play();
        }
    }
    public void StopLooping()
    {
        loop_state = false;
        audioSource.loop = loop_state;
    }
    
    public void StartLooping()
    {
        loop_state = true;
        audioSource.loop = loop_state;
    }

    public void ToggleLoop()
    {
        loop_state = !loop_state;
        audioSource.loop = !loop_state;
    }
    public void NextClip(bool process_state=false)
    {
        process[audioID] = audioSource.time;
        if (audioID+1 < soundClips.Count)
        {
            audioID += 1;
            audioSource.clip = soundClips[audioID];
        }
        else
        {
            audioID = 0;
            audioSource.clip = soundClips[audioID];
        }
        if (process_state)
        {
            audioSource.time = process[audioID];
        }
        audioSource.Play();
    }
    public void LastClip(bool process_state = false)
    {
        process[audioID] = audioSource.time;
        if (audioID - 1 >= 0)
        {
            audioID -= 1;
            audioSource.clip = soundClips[audioID];
        }
        else
        {
            audioID = soundClips.Count - 1;
            audioSource.clip = soundClips[audioID];
        }
        if (process_state)
        {
            audioSource.time = process[audioID];
        }
        audioSource.Play();
    }
    public void JumpTo(int id, bool process_state = false)
    {
        process[audioID] = audioSource.time;
        audioID = id;
        audioSource.clip = soundClips[audioID];
        if (process_state)
        {
            audioSource.time = process[audioID];
        }
        audioSource.Play();
    }
    public void Stop()
    {
        audioSource.Stop();
    }
    public void Play()
    {
        audioSource.Play();
    }
    public void StopOrPlay()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        else
        {
            audioSource.Play();
        }
       
    }
    public void ChangeProcess(int t)
    {
        audioSource.time = t;
    }
}
