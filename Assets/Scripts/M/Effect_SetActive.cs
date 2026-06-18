using System.Collections.Generic;
using UnityEngine;

public class Effect_SetActive : MonoBehaviour
{
    public List<GameObject> next;
    public Effect effect;
    public float delay_time;
    public bool PlayOnAwake = true;
    public bool PlayOnce = false;
    int state;
    float timer;
    int times;
    public enum Effect
    {
        Directly,
        Delayed
    }
    private void OnEnable()
    {
        if (!PlayOnAwake)
        {
            return;
        }
        if (PlayOnce)
        {
            if (times > 0)
            {
                return;
            }
            times++;
        }
        timer = 0;
        state = 0;
        Active();
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
                foreach (GameObject n in next)
                {
                    n.SetActive(true);
                }
            }
        }
    }
    public void Active()
    {
        if (effect == Effect.Directly)
        {
            foreach (GameObject n in next)
            {
                n.SetActive(true);
            }
        }
        else if (effect == Effect.Delayed)
        {
            state = 1;
        }
    }
}
