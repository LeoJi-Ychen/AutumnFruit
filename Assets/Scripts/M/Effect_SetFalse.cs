using UnityEngine;
using System.Collections.Generic;

public class Effect_SetFalse : MonoBehaviour
{
    public List<GameObject> next;
    public Effect effect;
    public float delay_time;
    int state;
    float timer;
    public enum Effect
    {
        Directly,
        Delayed
    }
    private void OnEnable()
    {
        timer = 0;
        state = 0;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        False();
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            timer += Time.deltaTime;
            if (timer > delay_time)
            {
                timer = 0;
                state = 0;
                foreach(GameObject n in next)
                {
                    n.SetActive(false);
                }           
            }
        }
    }
    void False()
    {
        if (effect == Effect.Directly)
        {
            foreach (GameObject n in next)
            {
                n.SetActive(false);
            }
        }
        else if (effect == Effect.Delayed)
        {
            state = 1;
        }
    }
}
