using UnityEngine;
using UnityEngine.InputSystem;

public class M_Fragment : MonoBehaviour
{
    public GameObject aim;
    public GameObject staticObj;
    public float triggerDistance;
    public Mode mode;
    IUF uf = new UIFunctions();
    int state;

    public enum Mode
    {
        Replace,
        Adsorption
    }
    private void Start()
    {
        if (triggerDistance <= 0)
        {
            triggerDistance = 0.5f;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(state == 1)
        {
            return;
        }
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (uf.Distance2(aim, this.gameObject.transform.position) < triggerDistance)
            {
                if(mode == Mode.Replace)
                {
                    this.gameObject.transform.position = aim.transform.position;
                    this.gameObject.SetActive(false);
                    if (staticObj != null)
                    {
                        staticObj.SetActive(true);
                    }           
                    state = 1;
                }
                if (mode == Mode.Replace)
                {
                    this.gameObject.transform.position = aim.transform.position;
                }
            }
        }
    }
}
