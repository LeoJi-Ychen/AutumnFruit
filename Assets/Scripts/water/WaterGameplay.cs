using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class WaterGameplay : MonoBehaviour
{
    public GameObject startButton;
    public List<GameObject> waterBubbles = new List<GameObject>();
    public float time_1;
    public float time_2;
    public float end_time;
    int progress;
    int startState;
    public List<GameObject> next = new List<GameObject>();
    [Header("完成事件")]
    public UnityEvent onPuzzleComplete;
    [Header("动画")]
    public RuntimeAnimatorController runtimeAnimatorController;
    public string idleName;
    public string playName;
    [Header("进度条")]
    public GameObject bar;
    public GameObject bar_line;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        startState = 0;
        progress = 0;
        InitGame();
        startButton.GetComponent<Button>().onClick.AddListener(StartGame);
        if (bar != null)
        {
            WaterProgressBar b = bar.AddComponent<WaterProgressBar>();
            b.bar = bar_line;
            bar.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitGame()
    {
        foreach (GameObject water in waterBubbles)
        {
            water.SetActive(false);
            WaterBubble c = water.AddComponent<WaterBubble>();
            c.gameplay = this;
            c.time_1 = time_1;
            c.time_2 = time_2;
            c.end_time = end_time;
            c.runtimeAnimatorController = runtimeAnimatorController;
            c.idleName = idleName;
            c.playName = playName;
            c.Init();
        }
    }
    void StartGame()
    {
        if(startState == 0)
        {
            startState = 1;
            if (bar != null)
            {
                bar.SetActive(true);
            }

            foreach (GameObject water in waterBubbles)
            {
                water.SetActive(true);
            }
            for (int i = 0; i < waterBubbles.Count; i++)
            {
                if (i == 0)
                {
                    waterBubbles[0].GetComponent<WaterBubble>().state = 1;
                }
                else
                {
                    waterBubbles[i].GetComponent<WaterBubble>().state = 0;
                }
            }          
        }      
    }
    public void NextStep()
    {
        progress++;
        if(progress >= waterBubbles.Count)
        {
            Win();
        }
        else
        {
            waterBubbles[progress].GetComponent<WaterBubble>().state = 1;
        }
        if (waterBubbles.Count > 0)
        {
            if (bar)
            {
                bar.GetComponent<WaterProgressBar>().progress = (float)progress / (float)waterBubbles.Count;
            }          
        }    
    }
    public void LoseGame()
    {
        startState = 0;
        progress = 0;
        foreach (GameObject water in waterBubbles)
        {
            water.SetActive(false);
        }
        if (bar)
        {
            bar.GetComponent<WaterProgressBar>().progress = 0;
            bar.SetActive(false);
        }
    }
    public void Win()
    {
        foreach(GameObject n in next)
        {
            n.SetActive(true);
        }
        onPuzzleComplete?.Invoke();
    }
}
