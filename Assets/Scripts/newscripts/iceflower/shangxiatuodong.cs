using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public bool startCanScroll = false;
    public float scrollSpeed = 5f, smoothSpeed = 8f;
    public float minY = 0f, maxY = 20f;

    float targetY;
    bool canScroll;

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

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, targetY, smoothSpeed * Time.deltaTime),
            transform.position.z
        );
    }

    public void OpenScroll() => canScroll = true;
    public void CloseScroll() => canScroll = false;
}