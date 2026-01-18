using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Effect_TurnBlack : MonoBehaviour
{
    float transparent;
    public  float clock;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        transparent = 0;
        if (clock <= 0)
        {
            clock = 3;
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Color cl = GetComponent<Image>().color;
        transparent = Mathf.SmoothStep(0f, 1f, timer/clock);
        cl.a = transparent; 
        GetComponent<Image>().color = cl;
    }
}
