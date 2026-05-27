using UnityEngine;

public class Effect_LimitRange : MonoBehaviour
{
    public Camera cam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float z = Mathf.Abs(cam.transform.position.z);
        Vector3 leftBottom = cam.ScreenToWorldPoint(new Vector3(0, 0, z));
        Vector3 rightTop = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, z));
        if (this.gameObject.transform.position.x > rightTop.x-0.1f)
        {
            this.gameObject.transform.position = new Vector2(rightTop.x - 0.1f, this.gameObject.transform.position.y);
        }
        if (this.gameObject.transform.position.y > rightTop.y - 0.1f)
        {
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, rightTop.y - 0.1f);
        }
        if (this.gameObject.transform.position.x < leftBottom.x + 0.1f)
        {
            this.gameObject.transform.position = new Vector2(leftBottom.x + 0.1f, this.gameObject.transform.position.y);
        }
        if (this.gameObject.transform.position.y < leftBottom.y + 0.1f)
        {
            this.gameObject.transform.position = new Vector2(this.gameObject.transform.position.x, leftBottom.y + 0.1f);
        }
    }
}
