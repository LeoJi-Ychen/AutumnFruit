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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startState = 0;
        progress = 0;
        InitGame();
        GetComponent<Button>().onClick.AddListener(StartGame);
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
            c.time_1 = time_1;
            c.time_2 = time_2;
            c.end_time = end_time;
        }
    }
    void StartGame()
    {
        startState = 1;
        for(int i=0; i<waterBubbles.Count;i++)
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
        foreach (GameObject water in waterBubbles)
        {
            water.SetActive(true);
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
    }
    public void LoseGame()
    {
        startState = 0;
        foreach (GameObject water in waterBubbles)
        {
            water.SetActive(false);
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
