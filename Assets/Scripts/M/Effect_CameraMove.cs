using System.Xml;
using UnityEngine;
using UnityEngine.UI;
using static Effect_ObjectMove;

public class Effect_CameraMove : MonoBehaviour
{
    Camera cam;

    [Header("开场自动播放")]
    public bool playOnStart = false;

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

    void Start()
    {
        if (!playOnStart)
            return;

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

    // 给Button、UnityEvent调用
    public void TriggerEffect()
    {
        if (effect == Trigger.Directly)
        {
            Effect();
        }
        else
        {
            wait_timer = 0;
            wait_state = 1;
        }
    }

    public void Effect()
    {
        if (clickstate == 1)
        {
            return;
        }

        clickstate = 1;

        if (cam == null)
        {
            Debug.LogError("没有找到 Main Camera");
            return;
        }

        Camera_SceneCamera sceneCam = cam.GetComponent<Camera_SceneCamera>();

        if (sceneCam == null)
        {
            Debug.LogError("Main Camera上没有挂Camera_SceneCamera组件");
            return;
        }

        if (sceneCam.state_move == 0)
        {
            if (md == mode.move)
            {
                sceneCam.state_move = 1;
                sceneCam.endPoint = endPoint;
                sceneCam.speed = speed;
            }
            else if (md == mode.jump)
            {
                sceneCam.state_move = 2;
                sceneCam.endPoint = endPoint;
            }
        }
    }
}