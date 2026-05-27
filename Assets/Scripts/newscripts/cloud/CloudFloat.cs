using UnityEngine;

public class CloudFloat : MonoBehaviour
{
    [Header("漂浮范围（小一点更自然）")]
    public float moveRange = 8f;   // 原来20 → 改小

    [Header("速度（慢一点更柔和）")]
    public float speed = 0.3f;

    [Header("附加效果")]
    public bool enableRotate = false; // 小幅漂浮建议关掉旋转
    public bool enableScale = true;

    private Vector3 startPos;
    private float offsetX;
    private float offsetY;

    void Start()
    {
        startPos = transform.position;

        offsetX = Random.Range(0f, 100f);
        offsetY = Random.Range(0f, 100f);

        speed *= Random.Range(0.9f, 1.1f);
        moveRange *= Random.Range(0.9f, 1.1f);
    }

    void Update()
    {
        float x = (Mathf.PerlinNoise(Time.time * speed + offsetX, 0f) - 0.5f) * 2f;
        float y = (Mathf.PerlinNoise(0f, Time.time * speed + offsetY) - 0.5f) * 2f;

        Vector3 offset = new Vector3(x, y, 0f) * moveRange;
        transform.position = startPos + offset;

        // 轻微呼吸感（很小）
        if (enableScale)
        {
            float scale = 1f + y * 0.02f;
            transform.localScale = new Vector3(scale, scale, 1f);
        }

        if (enableRotate)
        {
            transform.rotation = Quaternion.Euler(0, 0, x * 2f);
        }
    }
}