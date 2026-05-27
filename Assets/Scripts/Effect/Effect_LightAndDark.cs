using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Effect_LightAndDark : MonoBehaviour
{
    public GameObject effectObj;
    public float clock;
    float c_r;
    float c_g;
    float c_b;
    IUITools uitools;
    float timer;
    int p;
    public int state;

    void Start()
    {
        if (effectObj == null)
        {
            effectObj = this.gameObject;
        }
        p = 1;
        clock = 0.8f;
        uitools = new UITools();
        uitools.AddEntryEvent(effectObj);
        uitools.AddExitEvent(effectObj);
        effectObj.GetComponent<Image>().color = Color.white;
    }
    private void OnEnable()
    {
        if (effectObj == null)
        {
            effectObj = this.gameObject;
        }
        p = 1;
        timer = 0;
        effectObj.GetComponent<Image>().color = Color.white;
    }
    private void Update()
    {
        if (state == 0)
        {
            timer += p * Time.deltaTime;
            if (timer > clock)
            {
                timer = clock;
                p = -p;
            }
            if (timer < 0)
            {
                timer = 0;
                p = -p;
            }
            c_r = Mathf.SmoothStep(1f, 0.8f, timer / clock);
            c_g = Mathf.SmoothStep(1f, 0.8f, timer / clock);
            c_b = Mathf.SmoothStep(1f, 0.8f, timer / clock);
            Color cl = effectObj.GetComponent<Image>().color;
            cl.r = c_r;
            cl.g = c_g;
            cl.b = c_b;
            effectObj.GetComponent<Image>().color = cl;
        }
    }
}
