using UnityEngine;
using UnityEngine.UI;

public class WaterBubble : MonoBehaviour
{
    public WaterGameplay gameplay;
    public int state;
    public float time_1;
    public float time_2;
    public float end_time;
    float timer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }
    private void OnEnable()
    {
        timer = 0;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            timer += Time.deltaTime;        
        }
    }
    void Click()
    {
        if (state == 1)
        {
            if (timer >= time_1 && timer <= time_2)
            {
                state = 0;
                gameplay.NextStep();
            }
        }          
    }
}
