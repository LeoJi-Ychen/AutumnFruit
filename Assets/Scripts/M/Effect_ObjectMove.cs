using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Effect_ObjectMove : MonoBehaviour
{
    IUF uf = new UIFunctions();

    public GameObject obj;
    public GameObject endPoint;

    public mode md;

    public float speed = 1f;

    public Trigger effect;
    public float delayedClock;

    public bool playOnStart = false;

    int clickstate;
    float wait_timer = 0;
    int wait_state;

    [Header("全部完成事件")]
    public UnityEvent onComplete;
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

    void Start()
    {
        if (!playOnStart) return;

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
    void OnMoveComplete()
    {
        onComplete?.Invoke();
    }

    public void Effect()
    {
        if (clickstate == 1)
            return;

        clickstate = 1;

        Object_ObjectMove move = obj.GetComponent<Object_ObjectMove>();
        move.onMoveComplete.AddListener(OnMoveComplete);

        // ⭐ 起点
        move.posA = obj.transform.position;

        // ⭐ 终点
        move.endPoint = endPoint;

        // ⭐ speed → duration（统一系统）
        if (speed > 0)
        {
            float distance = Vector3.Distance(obj.transform.position, endPoint.transform.position);
            move.duration = distance / speed;
        }

        // ⭐ 启动移动
        if (move.state_move == 0)
        {
            if (md == mode.move)
            {
                move.state_move = 1;
            }
            else if (md == mode.jump)
            {
                move.state_move = 2;
            }
        }
    }
}