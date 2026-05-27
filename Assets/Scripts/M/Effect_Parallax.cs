using UnityEngine;

public class Effect_Parallax : MonoBehaviour
{
    [Header("Parallax Settings")]
    [SerializeField] private float parallaxStrength_X = 30f;   // 视差强度
    [SerializeField] private float parallaxStrength_Y = 0f;   // 视差强度
    [SerializeField] private float smoothSpeed = 5f;         // 平滑速度
    [SerializeField] private bool invert = false;            // 是否反向移动

    private Vector3 startPos;
    private Vector2 screenCenter;

    void Start()
    {
        startPos = transform.localPosition;
        screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
    }

    void Update()
    {
        Vector2 mousePos = Input.mousePosition;

        // 鼠标相对屏幕中心的偏移，归一化到 -1 ~ 1
        Vector2 offset = new Vector2(
            (mousePos.x - screenCenter.x) / screenCenter.x,
            (mousePos.y - screenCenter.y) / screenCenter.y
        );
        if (invert) { offset = -offset; }

        // 目标位置
        Vector3 targetPos = startPos + new Vector3(
            offset.x * parallaxStrength_X,
            offset.y * parallaxStrength_Y,
            0f
        );

        // 平滑移动
        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            targetPos,
            smoothSpeed * Time.deltaTime
        );
    }
}
