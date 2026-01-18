using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_Zoom : MonoBehaviour
{
    public int mode;
    public float mulPerSecX;
    public float mulPerSecY;
    public float maxTime;
    GameObject picture;
    Vector3 originScaling;
    float timer=0;
    // Start is called before the first frame update
    void Start()
    {
        picture = this.gameObject;
        originScaling = picture.GetComponent<RectTransform>().localScale;
        if (mode == 0)
        {
            mulPerSecX = 0.1f;
            mulPerSecY = 0.1f;
            maxTime = 3;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (timer < maxTime)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = maxTime;
        }
        float scale_x = (1 + mulPerSecX * timer) * originScaling.x;
        float scale_y = (1 + mulPerSecY * timer) * originScaling.y;
        picture.GetComponent<RectTransform>().localScale = new Vector3(scale_x, scale_y, 1);
    }
}
