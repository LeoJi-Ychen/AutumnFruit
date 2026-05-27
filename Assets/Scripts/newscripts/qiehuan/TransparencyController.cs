using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class TransparencyController : MonoBehaviour
{
    public List<GameObject> objs = new List<GameObject>();
    List<GameObject> gameObjects = new List<GameObject>();
    List<float> originAlpha = new List<float>();
    public float transparency;
    public float lastTransparency;
    public int state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        lastTransparency = transparency;
        foreach (GameObject g in objs)
        {
            GetChilds(g.transform);
        }
        foreach (GameObject g in objs)
        {
            gameObjects.Add(g);
        }
        foreach (GameObject g in gameObjects)
        {
            if (g.GetComponent<Image>())
            {
                originAlpha.Add(g.GetComponent<Image>().color.a);
            }
            else if (g.GetComponent<SpriteRenderer>())
            {
                originAlpha.Add(g.GetComponent<SpriteRenderer>().color.a);
            }
            else if (g.GetComponent<TextMeshProUGUI>())
            {
                originAlpha.Add(g.GetComponent<TextMeshProUGUI>().color.a);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 1)
        {
            state = 0;
            if (true)
            {
                lastTransparency = transparency;
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    if (gameObjects[i].GetComponent<Image>())
                    {
                        Color c = gameObjects[i].GetComponent<Image>().color;
                        c.a = Mathf.Lerp(0, originAlpha[i], transparency);
                        gameObjects[i].GetComponent<Image>().color = c;
                    }
                    else if (gameObjects[i].GetComponent<SpriteRenderer>())
                    {
                        Color c = gameObjects[i].GetComponent<SpriteRenderer>().color;
                        c.a = Mathf.Lerp(0, originAlpha[i], transparency);
                        gameObjects[i].GetComponent<SpriteRenderer>().color = c;
                    }
                    else if (gameObjects[i].GetComponent<TextMeshProUGUI>())
                    {
                        Color c = gameObjects[i].GetComponent<TextMeshProUGUI>().color;
                        c.a = Mathf.Lerp(0, originAlpha[i], transparency);
                        gameObjects[i].GetComponent<TextMeshProUGUI>().color = c;
                    }
                }
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
            gameObjects.Add(p.GetChild(i).gameObject);
            GetChilds(p.GetChild(i));
        }
    }
}
