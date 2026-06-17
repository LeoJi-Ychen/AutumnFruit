using UnityEngine;
using UnityEngine.UI;

public class OneClickButton : MonoBehaviour
{
    public Button button;

    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        Debug.Log("按钮只触发一次");

        // ⭐关键：禁用按钮
        button.interactable = false;
    }
}