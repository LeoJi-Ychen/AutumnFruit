using UnityEngine;

public class InitLoaction : MonoBehaviour
{
    public bool local = true;
    Vector3 localPos;
    Vector3 worldPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        localPos = transform.localPosition;
        worldPos = transform.position;
    }
    private void OnEnable()
    {
        Debug.Log(1);
        if (local)
        {
            transform.localPosition = localPos;
        }else
        {
            transform.position = worldPos;
        }
    }
}
