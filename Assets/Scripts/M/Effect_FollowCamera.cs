using UnityEngine;

public class Effect_FollowCamera : MonoBehaviour
{
    public bool mode;
    public Camera cam;
    public Vector2 offset;
    IUF uf = new Functions();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        cam = Camera.main;
        offset = (this.gameObject.transform.position-cam.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!mode)
        {
            this.gameObject.transform.position = (Vector2)cam.transform.position + offset;
        }
        else
        {
            this.gameObject.transform.position = (Vector2)cam.transform.position;
        }
        
    }
}
