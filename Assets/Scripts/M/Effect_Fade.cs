using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Effect_Appear;

public class Effect_Fade : MonoBehaviour
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

    public bool playOnStart = true;
    float times;
    public bool playOnce = true;
    public enum Kind
    {
        UI,
        Text,
        GameObject
    }
    public enum RaycastMode
    {
        False,
        ConditionalFalse,
        Active
    }
    public enum FinalMode
    {
        Close,
        Keep
    }
    public enum Effect
    {
        Directly,
        Delayed
    }
    public enum InitMode
    {
        Manual,
        Auto
    }

    private void OnEnable()
    {
        if (playOnce)
        {
            if (times > 0)
            {
                return;
            }
            times++;
        }
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
            transparent = 1;
            if (obj.GetComponent<Image>())
            {
                transparent = 1;
                Color cl = obj.GetComponent<Image>().color;
                cl.a = transparent;
                obj.GetComponent<Image>().color = cl;
            }
            else if (obj.GetComponent<SpriteRenderer>())
            {
                transparent = 1;
                Color cl = obj.GetComponent<SpriteRenderer>().color;
                cl.a = transparent;
                obj.GetComponent<SpriteRenderer>().color = cl;
            }
            else if (obj.GetComponent<TextMeshProUGUI>())
            {
                transparent = 1;
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
        g = new GameObject("Fade");
        g.SetActive(false);
        g.AddComponent<Fade>();
        Fade a = g.GetComponent<Fade>();
        switch (finalMode)
        {
            case FinalMode.Keep:
                a.finalMode = Fade.FinalMode.Keep;
                break;
            case FinalMode.Close:
                a.finalMode = Fade.FinalMode.Close;
                break;
        }
        switch (raycastMode)
        {
            case RaycastMode.ConditionalFalse:
                a.raycastMode = Fade.RaycastMode.ConditionalFalse;
                break;
            case RaycastMode.False:
                a.raycastMode = Fade.RaycastMode.False;
                break;
            case RaycastMode.Active:
                a.raycastMode = Fade.RaycastMode.Active;
                break;
        }
        a.clock = clock;
        a.threshold_transparent = threshold_transparent;
        a.obj = obj;
        if (wait_state == 1&&playOnStart)
        {
            g.SetActive(true);
            a.Init();
        }
    }

    void Start()
    {
       
    }

    // ✅ ⭐ 核心：外部触发入口（拼图 / 按钮 调这个）
    public void TriggerEffect()
    {
        Debug.Log("Fade 触发成功");
        if (g == null)
        {
            g = new GameObject("Fade");
            g.SetActive(false);
            g.AddComponent<Fade>();
            Fade a = g.GetComponent<Fade>();
            switch (finalMode)
            {
                case FinalMode.Keep:
                    a.finalMode = Fade.FinalMode.Keep;
                    break;
                case FinalMode.Close:
                    a.finalMode = Fade.FinalMode.Close;
                    break;
            }
            switch (raycastMode)
            {
                case RaycastMode.ConditionalFalse:
                    a.raycastMode = Fade.RaycastMode.ConditionalFalse;
                    break;
                case RaycastMode.False:
                    a.raycastMode = Fade.RaycastMode.False;
                    break;
                case RaycastMode.Active:
                    a.raycastMode = Fade.RaycastMode.Active;
                    break;
            }
            a.clock = clock;
            a.threshold_transparent = threshold_transparent;
            a.obj = obj;
        }      
        playOnStart = true;
        timer = 0;
        wait_timer = 0;

        // 👉 根据类型决定走不走延迟
        if (effect == Effect.Delayed)
        {
            wait_state = 0; // 先等 delay
        }
        else
        {
            wait_state = 1; // 直接开始
        }

        // 👉 确保物体是激活的
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        if (!obj.activeSelf)
            obj.SetActive(true);
        if (wait_state == 1 && playOnStart)
        {
            g.SetActive(true);
            g.GetComponent<Fade>().Init();
        }
    }

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
                    g.GetComponent<Fade>().Init();
                }
            }
        }
       
    }
}