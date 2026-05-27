using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class ControlChildTransparent : MonoBehaviour
{
    List<GameObject> childs = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetChilds(this.gameObject.transform);
        SetTransparent();
    }

    // Update is called once per frame
    void Update()
    {
        SetTransparent();
    }
    void SetTransparent()
    {
        float a = 0;
        if (GetComponent<Image>())
        {
            a = GetComponent<Image>().color.a;
        }
        else if (GetComponent<SpriteRenderer>())
        {
            a = GetComponent<SpriteRenderer>().color.a;
        }
        else if (GetComponent<TextMeshProUGUI>())
        {
            a = GetComponent<TextMeshProUGUI>().color.a;
        }

        foreach (GameObject child in childs)
        {
            if (child.GetComponent<Image>())
            {
                Color c = child.GetComponent<Image>().color;
                c.a = a;
                child.GetComponent<Image>().color = c;
            }
            else if (child.GetComponent<SpriteRenderer>())
            {
                Color c = child.GetComponent<SpriteRenderer>().color;
                c.a = a;
                child.GetComponent<SpriteRenderer>().color = c;
            }
            else if (child.GetComponent<TextMeshProUGUI>())
            {
                Color c = child.GetComponent<TextMeshProUGUI>().color;
                c.a = a;
                child.GetComponent<TextMeshProUGUI>().color = c;
            }
        }
    }
    void GetChilds(Transform p)
    {
        if (p.childCount <= 0)
        {
            return;
        }
        for (int i = 0; i < p.childCount; i++)
        {
            childs.Add(p.GetChild(i).gameObject);
            GetChilds(p.GetChild(i));
        }
    }
}
