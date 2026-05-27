using UnityEngine;

public class Plane_Move : MonoBehaviour
{
    public float speed;
    float mouseX;
    float mouseY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
    }

    void Update()
    {
        mouseX = Input.GetAxis("Mouse X");
        mouseY = Input.GetAxis("Mouse Y");
        if (mouseX>0)
        {
            transform.position += (Vector3)(Vector2.right * speed * Time.deltaTime);
        }
        if (mouseX<0)
        {
            transform.position += (Vector3)(Vector2.left * speed * Time.deltaTime);
        }
        if (mouseY>0)
        {
            transform.position += (Vector3)(Vector2.up * speed * Time.deltaTime);
        }
        if (mouseY<0)
        {
            transform.position += (Vector3)(Vector2.down * speed * Time.deltaTime);
        }
    }
}
