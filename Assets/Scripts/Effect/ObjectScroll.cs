using UnityEngine;

public class ObjectScroll : MonoBehaviour
{
    public bool startCanScroll = false;
    public bool canDrag = true;
    public bool resetWhenActive = true;
    public float scrollSpeed = 5f, smoothSpeed = 8f;
    public float dragSpeed = 0.05f;
    public float minY = 0f, maxY = 20f;
    float OriginY;

    float targetY;
    bool canScroll;

    private bool isDragging;
    private Vector3 lastMousePos;

    void Start()
    {
        OriginY = transform.position.y;
        targetY = transform.position.y;
        canScroll = startCanScroll;
    }
    private void OnEnable()
    {
        if (resetWhenActive)
        {
            targetY = OriginY;
        }
    }

    void Update()
    {
        HandleMouseWheel();
        if(canDrag)
        {
            HandleDrag();
        }

        transform.position = new Vector3(
            transform.position.x,
            Mathf.Lerp(transform.position.y, targetY, smoothSpeed * Time.deltaTime),
            transform.position.z
        );
    }
    void HandleMouseWheel()
    {
        float wheel = Input.GetAxis("Mouse ScrollWheel");

        if (wheel != 0)
        {
            targetY = Mathf.Clamp(
                targetY - wheel * scrollSpeed,
                minY,
                maxY
            );
        }
    }
    void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float deltaY =
                Input.mousePosition.y -
                lastMousePos.y;

            targetY = Mathf.Clamp(
                targetY + deltaY * dragSpeed,
                minY,
                maxY
            );

            lastMousePos = Input.mousePosition;
        }
    }


    public void OpenScroll() => canScroll = true;
    public void CloseScroll() => canScroll = false;
}