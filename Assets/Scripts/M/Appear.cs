using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Appear : MonoBehaviour
{
    float transparent;
    public float clock;
    public Effect effect;
    public float delayedClock;
    public RaycastMode raycastMode;
    public float threshold_transparent;
    public FinalMode finalMode;
    public GameObject obj;
    float timer;
    int state;
    public UnityEvent onAllComplete;
    public enum RaycastMode
    {
        ConditionalActive,
        False,
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
    public void Init()
    {
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
        if (clock <= 0)
        {
            transparent = 1;
        }
        if (obj.GetComponent<Image>())
        {
            if (threshold_transparent <= 0)
            {
                threshold_transparent = 0.95f;
            }
            if (raycastMode == RaycastMode.False)
            {
                obj.GetComponent<Image>().raycastTarget = false;
            }
            else if (raycastMode == RaycastMode.ConditionalActive)
            {
                obj.GetComponent<Image>().raycastTarget = false;
                state = 1;
            }
            else if (raycastMode == RaycastMode.Active)
            {
                obj.GetComponent<Image>().raycastTarget = true;
            }
        }
        //----------------------------------------------------------------------------------------------
        else if (obj.GetComponent<TextMeshProUGUI>())
        {
            if (threshold_transparent <= 0)
            {
                threshold_transparent = 0.95f;
            }
            if (raycastMode == RaycastMode.False)
            {
                obj.GetComponent<TextMeshProUGUI>().raycastTarget = false;
            }
            else if (raycastMode == RaycastMode.ConditionalActive)
            {
                obj.GetComponent<TextMeshProUGUI>().raycastTarget = false;
                state = 1;
            }
            else if (raycastMode == RaycastMode.Active)
            {
                obj.GetComponent<TextMeshProUGUI>().raycastTarget = true;
            }
        }
        else if (obj.GetComponent<SpriteRenderer>())
        {
            if (threshold_transparent <= 0)
            {
                threshold_transparent = 0.95f;
            }
        }
    }
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (clock > 0)
        {
            if(obj.GetComponent<Image>())
            {
                timer += Time.deltaTime;
                Color cl = obj.GetComponent<Image>().color;
                transparent = Mathf.SmoothStep(0f, 1f, timer / clock);
                cl.a = transparent;
                obj.GetComponent<Image>().color = cl;
                if (state == 1)
                {
                    if (transparent >= threshold_transparent)
                    {
                        obj.GetComponent<Image>().raycastTarget = true;
                        state = 0;
                    }
                }
                if (finalMode == FinalMode.Close)
                {
                    if (transparent > 0.99f)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            //-----------------------------------------------------------------------------------------------
            else if (obj.GetComponent<TextMeshProUGUI>())
            {
                timer += Time.deltaTime;
                Color cl = obj.GetComponent<TextMeshProUGUI>().color;
                transparent = Mathf.SmoothStep(0f, 1f, timer / clock);
                cl.a = transparent;
                obj.GetComponent<TextMeshProUGUI>().color = cl;
                if (state == 1)
                {
                    if (transparent >= threshold_transparent)
                    {
                        obj.GetComponent<TextMeshProUGUI>().raycastTarget = true;
                        state = 0;
                    }
                }
                if (finalMode == FinalMode.Close)
                {
                    if (transparent > 0.99f)
                    {
                        obj.SetActive(false);
                    }
                }
            }
            //-----------------------------------------------------------------------------------------------------
            else if (obj.GetComponent<SpriteRenderer>())
            {
                timer += Time.deltaTime;
                Color cl = obj.GetComponent<SpriteRenderer>().color;
                transparent = Mathf.SmoothStep(0f, 1f, timer / clock);
                cl.a = transparent;
                obj.GetComponent<SpriteRenderer>().color = cl;
                if (finalMode == FinalMode.Close)
                {
                    if (transparent > 0.99f)
                    {
                        obj.SetActive(false);
                    }
                }
            }
        }
        if (timer >= clock)
        {
            onAllComplete?.Invoke();
            Destroy(this.gameObject);
        }
    }
}
