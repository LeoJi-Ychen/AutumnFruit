using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Effect_Zoom : MonoBehaviour
{
    public ZoomMode zmode;
    Kind kind;
    public Effect effect;
    public GameObject center;
    public float finalScale_X;
    public float finalScale_Y;
    public float clock;
    public float delayedClock;
    public GameObject obj;
    Vector3 originPos;
    Vector3 originScaling;
    Vector2 originOffSet;
    Vector2 offSet;
    float timer = 0;
    float wait_timer = 0;
    int wait_state;
    Vector3 centerPos;
    public List<GameObject> next;
    int state;
    public enum ZoomMode
    {
        Smooth,
        Normal
    }
    public enum Kind
    {
        UI,
        GameObject
    }
    public enum Effect
    {
        Directly,
        Delayed
    }
    void Awake()
    {
        if (obj == null)
        {
            obj = this.gameObject;
        }
        if (obj.GetComponent<Image>())
        {
            kind = Kind.UI;
        }
        else
        {
            kind = Kind.GameObject;
        }
        if (kind == Kind.UI)
        {
            originScaling = obj.GetComponent<RectTransform>().localScale;
            originPos = obj.GetComponent<RectTransform>().position;
        }
        else if (kind == Kind.GameObject)
        {
            originScaling = obj.transform.localScale;
            originPos = obj.transform.position;
        }
        if (center != null)
        {
            centerPos = center.transform.position;
            originOffSet = (originPos - center.transform.position);
        }
    }
    // Start is called before the first frame update
    private void OnEnable()
    {
        wait_timer = 0;
        timer = 0;
        wait_state = 0;
        if (kind == Kind.UI)
        {
            obj.GetComponent<RectTransform>().localScale = originScaling;
            obj.GetComponent<RectTransform>().position = originPos;
        }
        else if (kind == Kind.GameObject)
        {
            obj.transform.localScale = originScaling;
            obj.transform.position = originPos;
        }
        if (effect == Effect.Directly)
        {
            wait_state = 1;
        }
        if (center != null)
        {
            originOffSet = (originPos - center.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            return;
        }
        float scale_x = originScaling.x;
        float scale_y = originScaling.y;
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
            timer += Time.deltaTime;
            if (timer < clock)
            {
                if (zmode == ZoomMode.Normal)
                {
                    scale_x = Mathf.Lerp(originScaling.x, finalScale_X, timer / clock);
                    scale_y = Mathf.Lerp(originScaling.y, finalScale_Y, timer / clock);
                }
                else if (zmode == ZoomMode.Smooth)
                {
                    scale_x = Mathf.SmoothStep(originScaling.x, finalScale_X, timer / clock);
                    scale_y = Mathf.SmoothStep(originScaling.y, finalScale_Y, timer / clock);
                }
            }
            else
            {
                wait_state = 2;
                scale_x = finalScale_X;
                scale_y = finalScale_Y;
            }
            if (kind == Kind.UI)
            {
                obj.GetComponent<RectTransform>().localScale = new Vector3(scale_x, scale_y, 1);
            }
            else if (kind == Kind.GameObject)
            {
                obj.transform.localScale = new Vector3(scale_x, scale_y, 1);
            }
            if (center != null)
            {
                offSet = new Vector2(originOffSet.x * Mathf.SmoothStep(1, 0, timer / clock), originOffSet.y * Mathf.SmoothStep(1, 0, timer / clock));
                if (kind == Kind.UI)
                {
                    obj.GetComponent<RectTransform>().position = centerPos + (Vector3)offSet;
                }
                else if (kind == Kind.GameObject)
                {
                    obj.transform.position = centerPos + (Vector3)offSet;
                }
            }
        }
        if(timer >= clock)
        {
            foreach(GameObject g in next)
            {
                if(g != null)
                {
                    g.SetActive(true);
                }
            }
            state = 1;
        }
    }
}
