using UnityEngine;
using System.Collections.Generic;
using TMPro;
public class M_Count : MonoBehaviour
{
    public List<GameObject> countlist;
    public int num;
    public TextMeshProUGUI display;
    public List<GameObject> next;
    int state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (display != null)
        {
            display.text = "" + num;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            return;
        }
        int n = 0;
        foreach(GameObject obj in countlist)
        {
            if (obj != null)
            {
                if (obj.activeSelf == true)
                {
                    n++;
                }
            }
        }
        num = n;
        if (display != null)
        {
            display.text = "" + num;
        }
        if (n == countlist.Count)
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
