using UnityEngine;
using UnityEngine.UI;
public class Button_ObjectMove : MonoBehaviour
{
    public GameObject obj;
    public GameObject endPoint;
    public mode md;
    public float speed;

    public enum mode
    {
        move,
        jump
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(Effect);
    }

    // Update is called once per frame
    void Effect()
    {
        if (obj.GetComponent<Object_ObjectMove>().state_move == 0)
        {
            if (md == mode.move)
            {
                obj.GetComponent<Object_ObjectMove>().state_move = 1;
                obj.GetComponent<Object_ObjectMove>().endPoint = endPoint;
                obj.GetComponent<Object_ObjectMove>().speed = speed;

            }
            else if (md == mode.jump)
            {
                obj.GetComponent<Object_ObjectMove>().state_move = 2;
                obj.GetComponent<Object_ObjectMove>().endPoint = endPoint;

            }
        }
    }
}
