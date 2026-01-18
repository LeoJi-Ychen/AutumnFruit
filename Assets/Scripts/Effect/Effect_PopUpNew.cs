using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Effect_PopUpNew : MonoBehaviour
{
    IUITools uitools;
    IUITools pop_uitools;
    public GameObject canvas;
    public GameObject popUp;
    GameObject pop;
    int state;
    int state_pop;
    float timer;

    // Start is called before the first frame update
    void Awake()
    {
        uitools = new UITools();
        pop_uitools = new UITools();
        uitools.AddEntryEvent(this.gameObject);
        uitools.AddExitEvent(this.gameObject);
    }

    private void OnEnable()
    {
        state = 0;
        popUp.SetActive(false);
    }
    void Update()
    {
        if (uitools.Entry())
        {
            if (pop == null)
            {
                state = 1;
                pop = GameObject.Instantiate(popUp, popUp.GetComponent<RectTransform>().parent);
                Vector2 wp = pop.GetComponent<RectTransform>().position;
                pop.GetComponent<RectTransform>().SetParent(canvas.transform);
                pop.GetComponent<RectTransform>().position = wp;
                pop_uitools.AddEntryEvent(pop);
                pop_uitools.AddExitEvent(pop);
                pop.SetActive(true);
            }
        }
        if (uitools.Exit())
        {
            state = 0;
        }
        if (pop != null)
        {
            if (pop_uitools.Entry())
            {
                state_pop = 1;
            }
            if (pop_uitools.Exit())
            {
                state_pop = 0;
            }
            if (state == 0 && state_pop == 0)
            {
                timer += Time.deltaTime;
                if (timer > 0.16f)
                {
                    timer = 0;
                    Destroy(pop);
                }
            }
            else
            {
                timer = 0;
            }
        }
    }
}
