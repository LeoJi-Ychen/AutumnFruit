using UnityEngine;

public class Object_ObjectMove : MonoBehaviour
{
    public int state_move;
    public float speed;
    public GameObject endPoint;
    IUF uf = new UIFunctions();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (state_move == 1)
        {
            if (uf.Distance2(this.gameObject, endPoint) > 0.03 * speed)
            {
                transform.Translate(uf.Direction2(this.gameObject, endPoint) * speed * Time.deltaTime);
            }
            else
            {
                state_move = 0;
                transform.position
                = new Vector3(endPoint.transform.position.x, endPoint.transform.position.y, transform.position.z);
            }
        }
        else if (state_move == 2)
        {
            state_move = 0;
            transform.position
               = new Vector3(endPoint.transform.position.x, endPoint.transform.position.y, transform.position.z);
        }
    }
}
