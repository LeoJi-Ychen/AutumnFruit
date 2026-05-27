using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChangeActive : MonoBehaviour
{
    public List<GameObject> activeObjects;
    public bool reset;
    int index;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Init();
        GetComponent<Button>().onClick.AddListener(Change);   
    }
    void Init()
    {
        for (int i = 0; i < activeObjects.Count; i++)
        {
            if (activeObjects[i].activeSelf == true)
            {
                index = i;
                break;
            }
        }
        for (int i = 0; i < activeObjects.Count; i++)
        {
            if (reset)
            {
                activeObjects[i].AddComponent<ResetPositionWhenActive>();
            }
            if (i != index)
            {
                activeObjects[i].SetActive(false);
            }
            else
            {
                activeObjects[i].SetActive(true);
            }
        }
    }
    void Change()
    {
        List<GameObject> list = new List<GameObject>(activeObjects);
        for(int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                activeObjects.Clear();
                for (int j = 0; j < list.Count; j++)
                {             
                    if (list[j] != null)
                    {
                        activeObjects.Add(list[j]);
                    }                
                }
                break;
            }
        }
        index++;
        if (index >= activeObjects.Count)
        {
            index = 0;
        }
        for(int i =0;i< activeObjects.Count;i++)
        {
            if (i != index)
            {
                activeObjects[i].SetActive(false);
            }
            else
            {
                activeObjects[i].SetActive(true);
            }
        }
    }
}
