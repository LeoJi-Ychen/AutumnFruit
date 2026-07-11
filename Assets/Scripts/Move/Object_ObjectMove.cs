using UnityEngine;
using UnityEngine.Events;

public class Object_ObjectMove : MonoBehaviour
{
    [HideInInspector] public int state_move;

    [HideInInspector] public Vector3 posA;
    [HideInInspector] public GameObject endPoint;

    public float duration = 2f;

    public GameObject next;

    // ⭐⭐⭐ 速度曲线（抛物线/加速/减速都在这里）
    public AnimationCurve speedCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    float time = 0f;
    public UnityEvent onMoveComplete;


    void Update()
    {
        if (state_move != 1) return;
        if (endPoint == null) return;

        time += Time.deltaTime;

        // 0~1 进度
        float t = Mathf.Clamp01(time / duration);

        // ⭐ 曲线控制“每一段速度”
        float curveT = speedCurve.Evaluate(t);

        // ⭐ 纯直线移动（重点）
        transform.position = Vector3.Lerp(posA, endPoint.transform.position, curveT);

        // 完成
        if (time >= duration)
        {
            transform.position = endPoint.transform.position;
            state_move = 0;

            if (next != null)
                next.SetActive(true);

            onMoveComplete?.Invoke();
        }
    }

    // ⭐ 开始移动
    public void StartMove()
    {
        if (endPoint == null) return;

        posA = transform.position;
        time = 0f;
        state_move = 1;
    }
}