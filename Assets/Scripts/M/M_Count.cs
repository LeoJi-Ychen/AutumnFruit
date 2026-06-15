using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Events;

public class M_Count : MonoBehaviour
{
    public List<GameObject> countlist;
    public int num;
    public TextMeshProUGUI display;

    [Header("完成后触发")]
    public UnityEvent trigger;

    int state;

    void Start()
    {
        if (display != null)
        {
            display.text = "" + num;
        }
    }

    void Update()
    {
        if (state == 1)
        {
            return;
        }

        int n = 0;

        foreach (GameObject obj in countlist)
        {
            if (obj != null && obj.activeSelf)
            {
                n++;
            }
        }

        num = n;

        if (display != null)
        {
            display.text = "" + num;
        }

        if (n == countlist.Count)
        {
            trigger?.Invoke();

            state = 1;
        }
    }
}