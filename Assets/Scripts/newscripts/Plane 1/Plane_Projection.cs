using UnityEngine;

public class Plane_Projection : MonoBehaviour
{
    public int state;
    public float speed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(state == 1)
        {
            transform.position = transform.position + (Vector3)(Vector2.right * speed * Time.deltaTime);
        }
    }
}
