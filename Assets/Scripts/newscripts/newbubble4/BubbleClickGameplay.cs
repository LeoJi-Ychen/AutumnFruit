using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BubbleClickGameplay : MonoBehaviour
{
    int state;
    public int totalCount;
    int currentCount;
    public bool reset;
    public List<GameObject> bubbles = new List<GameObject>();
    public List<float> spawnTime = new List<float>();
    public GameObject spawn;
    List<GameObject> spawnLocation = new List<GameObject>();
    int spawnIndex = 0;
    public List<GameObject> next = new List<GameObject>();
    public float existTime;
    public float triggerTime;
    float timer;
    [Header("Animator")]
    public RuntimeAnimatorController runtimeAnimatorController;
    public bool autoInit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        if (autoInit)
        {
            existTime = 0;
            foreach (AnimationClip clip in runtimeAnimatorController.animationClips)
            {
                existTime += clip.length;
            }
            triggerTime = 0.3f * existTime ;
        }
        for (int i = 0; i < spawn.transform.childCount; i++)
        {
            spawnLocation.Add(spawn.transform.GetChild(i).gameObject);
        }
        foreach (GameObject b in bubbles)
        {
            if (runtimeAnimatorController != null)
            {
                b.AddComponent<Animator>();
                b.GetComponent<Animator>().runtimeAnimatorController = runtimeAnimatorController;
            }
            BubbleClick c = b.AddComponent<BubbleClick>();
            c.existTime = existTime;
            c.triggerTime = triggerTime;
            c.gameplay = this;
            b.SetActive(false);
        }
        while (spawnTime.Count < bubbles.Count)
        {
            spawnTime.Add(0);
        }
    }
    public void AddCount()
    {
        currentCount++;
        Debug.Log(currentCount);
    }
    public void ResetCount()
    {
        if (reset)
        {
            currentCount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (state == 0)
        {
            for (int i = 0;i<bubbles.Count;i++)
            {
                GameObject b = bubbles[i];
                if (b.activeSelf == false)
                {
                    if (timer > spawnTime[i])
                    {
                        b.transform.position = spawnLocation[spawnIndex].transform.position;
                        spawnIndex += 1;
                        if (spawnIndex >= spawnLocation.Count)
                        {
                            spawnIndex = 0;
                        }
                        b.SetActive(true);
                    }
                }
            }
            if (currentCount >= totalCount)
            {
                state = 1;
                foreach (GameObject b in bubbles)
                {
                    b.SetActive(false);
                }
                foreach (GameObject n in next)
                {
                    n.SetActive(true);
                }
            }
        }
    }
}
