using UnityEngine;

public class ResetPositionWhenActive : MonoBehaviour
{
    int state = 0;
    Vector3 pos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        if(state == 0)
        {
            state = 1;
            pos = transform.position;
        }
        else
        {
            this.gameObject.transform.position = pos;
        }
        
    }
}
