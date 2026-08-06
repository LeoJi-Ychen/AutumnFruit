using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class Button_CameraMove : MonoBehaviour
{
    Camera cam;
    public GameObject endPoint;
    public mode md;
    public float speed;
    int state;
    public enum mode
    {
        move,
        jump
    }
    private void Awake()
    {
        cam = Camera.main;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Effect);
    }

    // Update is called once per frame
    void Effect()
    {
        if (state == 1)
        {
            return;
        }
        else
        {
            state = 1;
        }
        if(cam.GetComponent<Camera_SceneCamera>().state_move == 0)
        {
            if (md == mode.move)
            {
                cam.GetComponent<Camera_SceneCamera>().state_move = 1;
                cam.GetComponent<Camera_SceneCamera>().endPoint = endPoint;
                cam.GetComponent<Camera_SceneCamera>().speed = speed;

            }
            else if (md == mode.jump)
            {
                cam.GetComponent<Camera_SceneCamera>().state_move = 2;
                cam.GetComponent<Camera_SceneCamera>().endPoint = endPoint;

            }
        }        
    }
}
