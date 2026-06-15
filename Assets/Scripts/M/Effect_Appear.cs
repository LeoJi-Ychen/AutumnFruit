using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Effect_Appear : MonoBehaviour
{
    float transparent;
    public float clock;
    public Effect effect;
    public float delayedClock;
    Kind kind;
    public RaycastMode raycastMode;
    public float threshold_transparent;
    public FinalMode finalMode;
    public InitMode initMode;
    public GameObject obj;
    float timer;
    int state;
    int wait_state;
    float wait_timer;
    GameObject g;
    [Header("全部完成事件")]
    public UnityEvent onAllComplete;

    public bool playOnStart = true;
    public enum Kind
    {
        UI,
        Text,
        GameObject
    }
    public enum RaycastMode
    {
        False,
        ConditionalActive,
        Active
    }
    public enum FinalMode
    {
        Keep,
        Close
    }
    public enum Effect
    {
        Directly,
        Delayed
    }
    public enum InitMode
    {
        Auto,
        Manual
    }


    private void OnEnable()
    {
        if (obj == null)
        {
            obj = this.gameObject;
        }
        if (effect == Effect.Directly)
        {
            wait_state = 1;
        }
        if (initMode == InitMode.Auto)
        {
            transparent = 0;
            if (obj.GetComponent<Image>())
            {
                transparent = 0;
                Color cl = obj.GetComponent<Image>().color;
                cl.a = transparent;
                obj.GetComponent<Image>().color = cl;
            }
            else if (obj.GetComponent<SpriteRenderer>())
            {
                transparent = 0;
                Color cl = obj.GetComponent<SpriteRenderer>().color;
                cl.a = transparent;
                obj.GetComponent<SpriteRenderer>().color = cl;
            }
            else if (obj.GetComponent<TextMeshProUGUI>())
            {
                transparent = 0;
                Color cl = obj.GetComponent<TextMeshProUGUI>().color;
                cl.a = transparent;
                obj.GetComponent<TextMeshProUGUI>().color = cl;
            }
        }
        if (clock < 0)
        {
            transparent = 1;
        }
        else if (clock == 0)
        {
            clock = 3;
        }
        g = new GameObject("Appear");
        g.SetActive(false);
        g.AddComponent<Appear>();
        Appear a = g.GetComponent<Appear>();
        switch (finalMode)
        {
            case FinalMode.Keep:
                a.finalMode = Appear.FinalMode.Keep;
                break;
            case FinalMode.Close:
                a.finalMode = Appear.FinalMode.Close;
                break;
        }
        switch (raycastMode)
        {
            case RaycastMode.ConditionalActive:
                a.raycastMode = Appear.RaycastMode.ConditionalActive;
                break;
            case RaycastMode.False:
                a.raycastMode = Appear.RaycastMode.False;
                break;
            case RaycastMode.Active:
                a.raycastMode = Appear.RaycastMode.Active;
                break;
        }
        a.clock = clock;
        a.threshold_transparent = threshold_transparent;
        a.obj = obj;
        a.onAllComplete = onAllComplete;
        if (wait_state == 1 && playOnStart)
        {
            g.SetActive(true);
            a.Init();
        }
    }
    void Start()
    {

    }
    public void TriggerEffect()
    {
        Debug.Log("Appear 触发成功");
        if (g == null)
        {
            g = new GameObject("Appear");
            g.SetActive(false);
            g.AddComponent<Appear>();
            Appear a = g.GetComponent<Appear>();
            switch (finalMode)
            {
                case FinalMode.Keep:
                    a.finalMode = Appear.FinalMode.Keep;
                    break;
                case FinalMode.Close:
                    a.finalMode = Appear.FinalMode.Close;
                    break;
            }
            switch (raycastMode)
            {
                case RaycastMode.ConditionalActive:
                    a.raycastMode = Appear.RaycastMode.ConditionalActive;
                    break;
                case RaycastMode.False:
                    a.raycastMode = Appear.RaycastMode.False;
                    break;
                case RaycastMode.Active:
                    a.raycastMode = Appear.RaycastMode.Active;
                    break;
            }
            a.clock = clock;
            a.threshold_transparent = threshold_transparent;
            a.obj = obj;
            a.onAllComplete = onAllComplete;
        }
        playOnStart = true;
        timer = 0;
        wait_timer = 0;

        if (effect == Effect.Delayed)
            wait_state = 0;
        else
            wait_state = 1;

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        if (!obj.activeSelf)
            obj.SetActive(true);
        if (wait_state == 1 && playOnStart)
        {
            g.SetActive(true);
            g.GetComponent<Appear>().Init();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playOnStart)
        {
            if (clock > 0 && effect == Effect.Delayed)
            {
                if (wait_state == 0)
                {
                    wait_timer += Time.deltaTime;
                    if (wait_timer > delayedClock)
                    {
                        wait_timer = 0;
                        wait_state = 1;
                    }
                }
                else if (wait_state == 1)
                {
                    wait_state = 2;
                    g.SetActive(true);
                    g.GetComponent<Appear>().Init();
                }
            }
        }          
    }
}
