using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Effect_UIRotate : MonoBehaviour
{
    public int state;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Click);
    }
    void Click()
    {
        if (state == 0)
        {
            state = 1;
            GetComponent<RectTransform>().Rotate(0, 0, 90, Space.Self);
        }
        else
        {
            state = 0;
            GetComponent<RectTransform>().Rotate(0, 0, -90, Space.Self);
        }
    }
}
