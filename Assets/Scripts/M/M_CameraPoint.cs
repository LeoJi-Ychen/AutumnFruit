using UnityEngine;
using System.Collections.Generic;
public class M_CameraPoint : MonoBehaviour
{
    Camera cam;
    public List<GameObject> next;
    int state;
    IUF uf = new UIFunctions();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0)
        {
            if(uf.Distance2(this.gameObject, cam.transform.position) < 0.1f)
            {
                foreach(GameObject g in next)
                {
                    if (g != null)
                    {
                        g.SetActive(true);
                    }
                }
                state = 1;
            }
        }
    }
}
