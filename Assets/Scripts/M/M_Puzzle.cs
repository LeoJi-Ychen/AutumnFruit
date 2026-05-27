using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class M_Puzzle : MonoBehaviour
{
    public Mode mode;
    public List<GameObject> fragments = new List<GameObject>();
    public List<GameObject> aims = new List<GameObject>();
    IUF uf = new UIFunctions();
    bool success = false;
    public GameObject next;
    public List<GameObject> nextList = new List<GameObject>();
    public List<GameObject> activeList = new List<GameObject>();
    int state;
    int activeIndex;
    int lastActiveIndex;

    public enum Mode
    {
        Normal,
        Active,
        Trigger
    }
    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            return;
        }
        if(mode != Mode.Trigger)
        {
            if (Mouse.current.leftButton.isPressed)
            {
                return;
            }
        }
        if (mode == Mode.Normal)
        {
            success = true;
            for (int i = 0; i < fragments.Count; i++)
            {
                if (uf.Distance2(fragments[i], aims[i]) > 0.1f)
                {
                    success = false;
                    break;
                }
            }
            if (activeList.Count > 0)
            {
                activeIndex = 0;
                for (int i = 0; i < fragments.Count; i++)
                {
                    if (uf.Distance2(fragments[i], aims[i]) < 0.1f)
                    {
                        activeIndex++;
                    }
                }
                if (activeIndex > lastActiveIndex)
                {
                    if (lastActiveIndex < activeList.Count)
                    {
                        activeList[lastActiveIndex].SetActive(true);
                    }
                    lastActiveIndex = activeIndex;
                }
            }
        }
        if (mode == Mode.Active|| mode == Mode.Trigger)
        {
            success = true;
            for (int i = 0; i < fragments.Count; i++)
            {
                if (uf.Distance2(fragments[i], aims[i]) > 0.1f || !fragments[i].activeSelf)
                {
                    success = false;
                    break;
                }
            }
            if (activeList.Count > 0)
            {
                activeIndex = 0;
                for (int i = 0; i < fragments.Count; i++)
                {
                    if (uf.Distance2(fragments[i], aims[i]) < 0.1f || !fragments[i].activeSelf)
                    {
                        activeIndex++;
                    }
                }
                if (activeIndex > lastActiveIndex)
                {
                    if (lastActiveIndex < activeList.Count)
                    {
                        activeList[lastActiveIndex].SetActive(true);
                    }
                    lastActiveIndex = activeIndex;
                }
            }
        }
        if (success)
        {
            state = 1;
            next.SetActive(true);
            foreach (GameObject go in nextList)
            {
                go.SetActive(true);
            }
        }
    }
}
