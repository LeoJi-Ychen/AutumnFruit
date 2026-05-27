using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Effect_Button : MonoBehaviour
{
    public GameObject obj;
    public float multi;
    Vector2 initSize;
    IUITools uitools;

    void Awake()
    {
        uitools = new UITools();
        if (multi <= 0)
        {
            multi = 1.1f;
        }
        //initParent = GetComponent<RectTransform>().parent;
        if (obj == null)
        {
            initSize = GetComponent<RectTransform>().localScale;
            obj = this.gameObject;
        }
        else
        {
            initSize = obj.GetComponent<RectTransform>().localScale;
        }
        uitools.AddEntryEvent(obj);
        uitools.AddExitEvent(obj);
    }

    private void OnEnable()
    {
        if (obj == null)
        {
            GetComponent<RectTransform>().localScale = initSize;
        }
        else
        {
            obj.GetComponent<RectTransform>().localScale = initSize;
        }
    }
    private void Update()
    {
        if (uitools.Entry())
        {
            if (obj == null)
            {
                GetComponent<RectTransform>().localScale = initSize * multi;
            }
            else
            {
                obj.GetComponent<RectTransform>().localScale = initSize * multi;
            }
        }
        if (uitools.Exit())
        {
            if (obj == null)
            {
                GetComponent<RectTransform>().localScale = initSize;
            }
            else
            {
                obj.GetComponent<RectTransform>().localScale = initSize;
            }
        }
    }
}
