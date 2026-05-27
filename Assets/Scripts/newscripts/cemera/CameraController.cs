using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Camera cam;
    private bool isMoving = false;

    private Vector3 targetPos;
    private float targetZoom;

    void Start()
    {
        cam = Camera.main;
        targetPos = cam.transform.position;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        if (!isMoving) return;

        cam.transform.position = Vector3.Lerp(
            cam.transform.position,
            targetPos,
            Time.deltaTime * moveSpeed
        );

        cam.orthographicSize = Mathf.Lerp(
            cam.orthographicSize,
            targetZoom,
            Time.deltaTime * moveSpeed
        );

        if (Vector3.Distance(cam.transform.position, targetPos) < 0.01f &&
            Mathf.Abs(cam.orthographicSize - targetZoom) < 0.01f)
        {
            isMoving = false;
        }
    }

    public void SetTarget(Transform point, float size)
    {
        targetPos = new Vector3(point.position.x, point.position.y, cam.transform.position.z);
        targetZoom = size;
        isMoving = true;
    }
}
