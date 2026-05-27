using UnityEngine;

public class CameraButton : MonoBehaviour
{
    public CameraController cam;
    public Transform targetPoint;
    public float targetSize = 5f;

    public void OnClick()
    {
        cam.SetTarget(targetPoint, targetSize);
    }
}
