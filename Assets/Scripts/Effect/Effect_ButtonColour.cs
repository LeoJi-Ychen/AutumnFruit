using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Effect_ButtonColour : MonoBehaviour
{
    public List<GameObject> button=new List<GameObject>();
    public int mode;//0 red 1 green 2 yellow 3 blue
    List<Color> initColor;
    IUITools uitools;

    void Start()
    {
        uitools = new UITools();
        initColor = new List<Color>();
        if (button.Count <= 0)
        {
            button.Add(this.gameObject);
        }
        for (int i = 0; i < button.Count; i++)
        {
            initColor.Add(button[i].GetComponent<Image>().color);
        }
        uitools.AddEntryEvent(this.gameObject);
        uitools.AddExitEvent(this.gameObject);
    }
    private void OnEnable()
    {
        if (button.Count <= 0)
        {
            button.Add(this.gameObject);
        }
        for (int i = 0; i < button.Count; i++)
        {
            button[i].GetComponent<Image>().color = initColor[i];
        }
    }
    private void Update()
    {
        if (uitools.Entry())
        {
            if (mode == 0)
            {
                for (int i = 0; i < button.Count; i++)
                {
                    button[i].GetComponent<Image>().color = Color.red;
                }
            }
            if (mode == 1)
            {
                for (int i = 0; i < button.Count; i++)
                {
                    button[i].GetComponent<Image>().color = Color.green;
                }
            }
            if (mode == 2)
            {
                for (int i = 0; i < button.Count; i++)
                {
                    button[i].GetComponent<Image>().color = Color.yellow;
                }
            }
            if (mode == 3)
            {
                for (int i = 0; i < button.Count; i++)
                {
                    button[i].GetComponent<Image>().color = Color.blue;
                }
            }
        }
        if (uitools.Exit())
        {
            for (int i = 0; i < button.Count; i++)
            {
                button[i].GetComponent<Image>().color = initColor[i];
            }
        }
    }
}
