using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Camera_SceneCamera : MonoBehaviour
{
    EventSystem es;
    public int state_move;
    public float speed;
    public GameObject endPoint;
    IUF uf = new UIFunctions();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        es = EventSystem.current;
    }

    // Update is called once per frame
    void Update()
    {
        if(state_move == 1)
        {
            if (uf.Distance2(this.gameObject, endPoint) > 0.03 * speed)
            {
                transform.Translate(uf.Direction2(this.gameObject, endPoint) * speed * Time.deltaTime);
                es.enabled = false;
            }
            else
            {
                state_move = 0;
                transform.position
                = new Vector3(endPoint.transform.position.x, endPoint.transform.position.y, transform.position.z);
                es.enabled = true;
            }
        }
        else if(state_move == 2)
        {
            state_move = 0;
            transform.position
               = new Vector3(endPoint.transform.position.x, endPoint.transform.position.y, transform.position.z);
        }        
    }
}
