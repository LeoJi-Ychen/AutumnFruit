using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using static Effect_ObjectMove;
public class Effect_CameraMove : MonoBehaviour
{
    Camera cam;
    public GameObject endPoint;
    public mode md;
    public float speed;
    public Trigger effect;
    public float delayedClock;
    int clickstate;
    float wait_timer = 0;
    int wait_state;
    public enum mode
    {
        move,
        jump
    }
    public enum Trigger
    {
        Directly,
        Delayed
    }
    private void Awake()
    {
        cam = Camera.main;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (effect == Trigger.Directly)
        {
            Effect();
        }
        else
        {
            wait_state = 1;
        }
    }
    void Update()
    {
        if (wait_state == 1)
        {
            wait_timer += Time.deltaTime;
            if (wait_timer > delayedClock)
            {
                wait_timer = 0;
                wait_state = 0;
                Effect();
            }
        }
    }

    // Update is called once per frame
    void Effect()
    {
        if (clickstate == 1)
        {
            return;
        }
        else
        {
            clickstate = 1;
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
