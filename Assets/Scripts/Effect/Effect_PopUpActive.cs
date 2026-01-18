using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Effect_PopUpActive : MonoBehaviour
{
    public int mode;
    IUITools uitools;
    IUITools pop_uitools;
    public GameObject popUp;
    int state;

    // Start is called before the first frame update
    void Awake()
    {
        uitools = new UITools();
        pop_uitools = new UITools();
        uitools.AddEntryEvent(this.gameObject);
        uitools.AddExitEvent(this.gameObject);
        if (mode == 0)
        {
            uitools.AddButtonClick(this.gameObject);
            pop_uitools.AddButtonClick(popUp);
        }
    }

    private void OnEnable()
    {
        state = 0;
        popUp.SetActive(false);
    }
    private void Update()
    {
        if (uitools.Entry())
        {
            state = 1;
            popUp.SetActive(true);
        }
        if (uitools.Exit())
        {
            state = 0;
            popUp.SetActive(false);
        }
        if (uitools.ButtonClicked()|| pop_uitools.ButtonClicked())
        {
            Click();
        }
    }
    public void Click()
    {
        if (state == 1)
        {
            state = 0;
            popUp.SetActive(false);
        }
        else
        {
            state = 1;
            popUp.SetActive(true);
        }
    }
}
