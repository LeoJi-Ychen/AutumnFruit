using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect_RotateAroundPoint : MonoBehaviour
{
    // 绕着旋转的点
    public Vector3 pivotPoint;
    public Vector3 localPos;
    // 旋转的轴（在2D中通常是Z轴）
    public Vector3 rotationAxis = Vector3.forward;
    // 每秒旋转的角度
    public float rotationSpeed = -120f;
    public void Start()
    {
        rotationSpeed = -120f;
        localPos = transform.localPosition;
        pivotPoint = transform.position;
    }
    private void LateUpdate()
    {// 计算旋转角度
        float angle = rotationSpeed * Time.deltaTime;
        transform.localPosition = localPos;
        pivotPoint = transform.position;
        // 使用Transform.RotateAround方法让物体绕指定点旋转
        transform.RotateAround(pivotPoint, rotationAxis, angle);
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.up = Vector2.up;
        }
    }
}
