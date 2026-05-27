using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Effect_Flashing : MonoBehaviour
{
    public GameObject effectObj;
    public float clock;
    float transparent;
    IUITools uitools;
    float timer;
    int p;

    void Awake()
    {
        p = 1;
        clock = 1.0f;
        transparent = 0f;
        uitools = new UITools();
        uitools.AddEntryEvent(this.gameObject);
        uitools.AddExitEvent(this.gameObject);
        Color cl = effectObj.GetComponent<Image>().color;
        cl.a = transparent;
        effectObj.GetComponent<Image>().color = cl;
    }
    private void OnEnable()
    {
        p = 1;
        timer = 0;
        transparent = 0f;
        Color cl = effectObj.GetComponent<Image>().color;
        cl.a = transparent;
        effectObj.GetComponent<Image>().color = cl;
    }
    private void Update()
    {
        if (uitools.Stay())
        {
            timer += p * Time.deltaTime;
            if (timer > clock)
            {
                timer = clock;
                p = -p;
            }
            if (timer <0)
            {
                timer = 0;
                p = -p;
            }
            transparent = Mathf.SmoothStep(0, 0.7f, timer / clock);
            Color cl = effectObj.GetComponent<Image>().color;
            cl.a = transparent;
            effectObj.GetComponent<Image>().color = cl;
        }
        if (uitools.Exit())
        {
            p = 1;
            timer = 0;
            transparent = 0f;
            Color cl = effectObj.GetComponent<Image>().color;
            cl.a = transparent;
            effectObj.GetComponent<Image>().color = cl;
        }
    }
}
