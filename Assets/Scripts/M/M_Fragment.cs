using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class M_Fragment : MonoBehaviour
{
    public GameObject aim;
    public float triggerDistance;
    public float AdsorptionSpeed;
    IUF uf = new UIFunctions();
    public int state;
    public List<GameObject> next;
    public bool NotSetFalse = false;


    private void Start()
    {
        state = 0;
        if (triggerDistance <= 0)
        {
            triggerDistance = 0.5f;
        }
        if(AdsorptionSpeed <= 0)
        {
            AdsorptionSpeed = 3;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(state == 3)
        {
            return;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame&&state==0)
        {
            Debug.Log(uf.Distance2(aim, this.gameObject));
            if (uf.Distance2(aim, this.gameObject) < triggerDistance)
            {
                state = 1;                
            }
        }
        if (state==1)
        {
            this.gameObject.transform.position = 
                this.gameObject.transform.position + (Vector3)uf.Direction2(this.gameObject, aim)*AdsorptionSpeed*Time.deltaTime;
            if (uf.Distance2(aim, this.gameObject) < 0.1f * triggerDistance){
                state = 2;
                this.gameObject.transform.position = aim.transform.position;
            }
        }
        if (state == 2)
        {
            state = 3;
            this.gameObject.SetActive(false);
            if (NotSetFalse)
            {
                state = 0;
                this.gameObject.SetActive(true);
            }
            foreach(GameObject g in next)
            {
                g.SetActive(true);
            }
        }
    }
}
