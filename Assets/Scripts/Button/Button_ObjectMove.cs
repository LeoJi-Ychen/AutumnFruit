using UnityEngine;
using UnityEngine.UI;

public class Button_ObjectMove : MonoBehaviour
{
    IUF uf = new UIFunctions();

    public GameObject obj;
    public GameObject endPoint;

    public mode md;

    public float speed = 1f;

    int state;

    public enum mode
    {
        move,
        jump
    }

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Effect);
    }

    void Effect()
    {
        if (state == 1) return;
        state = 1;

        Object_ObjectMove move = obj.GetComponent<Object_ObjectMove>();

        // ⭐ 起点
        move.posA = obj.transform.position;

        // ⭐ 终点
        move.endPoint = endPoint;

        // ⭐ speed → duration（核心统一）
        if (speed > 0)
        {
            float distance = Vector3.Distance(obj.transform.position, endPoint.transform.position);
            move.duration = distance / speed;
        }

        // ⭐ 开始移动
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