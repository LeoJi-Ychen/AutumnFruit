using UnityEngine;

public class Effect_Hide : MonoBehaviour
{
    IUITools uitools;
    public GameObject hide;

    // Start is called before the first frame update
    void Awake()
    {
        uitools = new UITools();
        uitools.AddEntryEvent(this.gameObject);
        uitools.AddExitEvent(this.gameObject);
    }

    private void OnEnable()
    {
        hide.SetActive(true);
    }
    private void Update()
    {
        if (uitools.Entry())
        {
            hide.SetActive(false);
        }
        if (uitools.Exit())
        {
            hide.SetActive(true);
        }
    }
}
