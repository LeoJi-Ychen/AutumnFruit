using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UITools : IUITools
{
    public int mouseEnter;
    public int mouseExit;
    public int mouseStay;
    public int buttonDown;
    public int siblingIndex;
    public void AddEntryEvent(GameObject obj)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((data) => { mouseEnter = 1; mouseStay = 1; mouseExit = 0; });
        EventTrigger trigger = obj.gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(entry);
    }
    public void AddExitEvent(GameObject obj)
    {
        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = EventTriggerType.PointerExit;
        exit.callback.AddListener((data) => { mouseEnter = 0;mouseStay = 0; mouseExit = 1; });
        EventTrigger trigger = obj.gameObject.AddComponent<EventTrigger>();
        trigger.triggers.Add(exit);
    }
    public void AddButtonClick(GameObject button)
    {
        button.GetComponent<Button>().onClick.AddListener(ButtonDown);
    }
    public bool Entry()
    {
        if (mouseEnter == 1)
        {
            mouseEnter = 0;
            return true;
        }
        return false;
    }
    public bool Exit()
    {
        if (mouseExit == 1)
        {
            mouseExit = 0;
            return true;
        }
        return false;
    }
    public bool Stay()
    {
        if (mouseStay == 1)
        {
            return true;
        }
        return false;
    }
    void ButtonDown()
    {
        buttonDown = 1;
    }
    public bool ButtonClicked()
    {
        if (buttonDown == 1)
        {
            buttonDown = 0;
            return true;
        }
        return false;
    }
    public void RecordSiblingIndex(GameObject obj)
    {
        siblingIndex = obj.transform.GetSiblingIndex();
    }
    public int GetOriginSiblingIndex()
    {
        return siblingIndex;
    }
    public void SetSiblingBack(GameObject obj)
    {
        obj.transform.SetSiblingIndex(siblingIndex);
    }
    public void SetAsLastSibling(GameObject obj)
    {
        obj.transform.SetAsLastSibling();
    }
    public void AlphaRaycastImage(GameObject obj, float a = 0.1f)
    {
        obj.GetComponent<Image>().alphaHitTestMinimumThreshold = a;
    }
}
