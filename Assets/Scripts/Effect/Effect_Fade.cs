using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Effect_Fade : MonoBehaviour
{
    float transparent;
    public float clock;
    float timer;
    // Start is called before the first frame update
    void Start()
    {
        transparent = 1;
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
        transparent = Mathf.SmoothStep(1f, 0f, timer / clock);
        cl.a = transparent;
        GetComponent<Image>().color = cl;
        if (transparent < 0.36f)
        {
            GetComponent<Image>().raycastTarget = false;
        }
        if (transparent <= 0.01f)
        {
            this.gameObject.SetActive(false);
        }
    }
}
