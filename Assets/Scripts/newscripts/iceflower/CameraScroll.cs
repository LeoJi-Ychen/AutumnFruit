using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class CameraScroll : MonoBehaviour
{
    public bool startCanScroll = false;
    public float scrollSpeed = 5f, smoothSpeed = 8f;
    public float dragSpeed = 0.02f;     // 拖动灵敏度
    public float minY = 0f, maxY = 20f;

    float targetY;
    bool canScroll;
    bool mouseDrag = true;
    bool bottomCheck = false;
    Vector3 lastMousePos;

    void Start()
    {
        targetY = transform.position.y;
        canScroll = startCanScroll;
    }

    void Update()
    {
        if (!canScroll) return;

        float w = Input.GetAxis("Mouse ScrollWheel");

        if (w != 0)
            targetY = Mathf.Clamp(targetY + w * scrollSpeed, minY, maxY);
        if (mouseDrag)
        {
            if (Input.GetMouseButtonDown(0))
            {
                lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0))
            {
                float deltaY = Input.mousePosition.y - lastMousePos.y;

                // 鼠标向上拖，相机向上移动
                targetY = Mathf.Clamp(
                    targetY - deltaY * dragSpeed,
                    minY,
                    maxY
                );

                lastMousePos = Input.mousePosition;
            }
        }
        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, targetY, smoothSpeed * Time.deltaTime),
            transform.position.z
        );
        CheckBottomEdge();
    }

    public void OpenScroll() => canScroll = true;
    public void CloseScroll() => canScroll = false;

    public void SetMinY(float y)
    {
        minY = y;
    }
    public void SetMaxY(float y)
    {
        maxY = y;
    }
    public void MoveDirectly(float distance)
    {
        targetY += distance;
    }
    void CheckBottomEdge()
    {
        if ((transform.position.y - minY) <= 0.1f)
        {
            transform.position = new Vector3(transform.position.x,minY,transform.position.z);
            targetY = minY;
            canScroll = false;
        }
    }
    public void CheckBottom(bool bc)
    {
        bottomCheck = bc;
    }
}