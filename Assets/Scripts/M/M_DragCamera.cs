using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class M_DragCamera : MonoBehaviour
{
    public bool horizon;
    public bool Vertical;
    public GameObject EdgePoint_a;
    public GameObject EdgePoint_b;
    Vector2 pos_down;
    Vector2 pos_current;
    Vector2 offset;
    Camera cam;
    IUITools ut = new UITools();
    int state;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
        ut.AddEntryEvent(this.gameObject);
        ut.AddEntryEvent(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (IsMouseOverTarget())
            {
                state = 1;
            }
            else
            {
                state = 0;
            }
        }        
        if (ut.Stay()&&IsMouseOverTarget()&&state==1)
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                pos_down = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            }
            if (Mouse.current.leftButton.isPressed)
            {
                pos_current = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                offset = pos_down - pos_current;
                if (horizon)
                {
                    cam.transform.position = cam.transform.position + new Vector3(offset.x, 0, 0);
                    if(EdgePoint_a != null)
                    {
                        float minx = Mathf.Min(EdgePoint_a.transform.position.x, EdgePoint_b.transform.position.x);
                        float maxx = Mathf.Max(EdgePoint_a.transform.position.x, EdgePoint_b.transform.position.x);
                        if (cam.transform.position.x > maxx)
                        {
                            cam.transform.position = new Vector3(maxx, cam.transform.position.y, cam.transform.position.z);
                        }
                        if (cam.transform.position.x < minx)
                        {
                            cam.transform.position = new Vector3(minx, cam.transform.position.y, cam.transform.position.z);
                        }
                    }                  
                }
                if (Vertical)
                {
                    cam.transform.position = cam.transform.position + new Vector3(0, offset.y, 0);
                    if (EdgePoint_a != null)
                    {
                        float miny = Mathf.Min(EdgePoint_a.transform.position.y, EdgePoint_b.transform.position.y);
                        float maxy = Mathf.Max(EdgePoint_a.transform.position.y, EdgePoint_b.transform.position.y);
                        if (cam.transform.position.y > maxy)
                        {
                            cam.transform.position = new Vector3(cam.transform.position.x, maxy, cam.transform.position.z);
                        }
                        if (cam.transform.position.y < miny)
                        {
                            cam.transform.position = new Vector3(cam.transform.position.x, miny, cam.transform.position.z);
                        }
                    }
                }            
            }
        }      
    }
    bool IsMouseOverTarget()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, results);

        if (results.Count == 0)
            return false;

        return results[0].gameObject == this.gameObject;
    }
}
