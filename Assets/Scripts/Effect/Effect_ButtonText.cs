using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Effect_ButtonText : MonoBehaviour
{
    public List<TextMeshProUGUI> buttonText;
    public int mode;//0 red 1 green 2 yellow 3 blue
    List<Color> initColor;
    IUITools uitools;

    void Awake()
    {
        uitools = new UITools();
        initColor = new List<Color>();
        if (buttonText.Count<=0)
        {
            buttonText.Add(this.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>());
        }
        for(int i = 0; i < buttonText.Count; i++)
        {
            initColor.Add(buttonText[i].color);
        }
        uitools.AddEntryEvent(this.gameObject);
        uitools.AddExitEvent(this.gameObject);
    }
    private void OnEnable()
    {
        for (int i = 0; i < buttonText.Count; i++)
        {
            buttonText[i].color = initColor[i];
            buttonText[i].fontStyle = FontStyles.Normal;
        }       
    }
    private void Update()
    {
        if (uitools.Entry())
        {
            if (mode == 0)
            {
                for (int i = 0; i < buttonText.Count; i++)
                {
                    buttonText[i].color = Color.red;
                    buttonText[i].fontStyle = FontStyles.Bold;
                }
            }
            if (mode == 1)
            {
                for (int i = 0; i < buttonText.Count; i++)
                {
                    buttonText[i].color = Color.green;
                    buttonText[i].fontStyle = FontStyles.Bold;
                }
            }
            if (mode == 2)
            {
                for (int i = 0; i < buttonText.Count; i++)
                {
                    buttonText[i].color = Color.yellow;
                    buttonText[i].fontStyle = FontStyles.Bold;
                }
            }
            if (mode == 3)
            {
                for (int i = 0; i < buttonText.Count; i++)
                {
                    buttonText[i].color = Color.blue;
                    buttonText[i].fontStyle = FontStyles.Bold;
                }
            }
        }
        if (uitools.Exit())
        {
            for (int i = 0; i < buttonText.Count; i++)
            {
                buttonText[i].color = initColor[i];
                buttonText[i].fontStyle = FontStyles.Normal;
            }
        }
    }
}
