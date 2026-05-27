using UnityEngine;

public interface IUITools
{
    void AddEntryEvent(GameObject obj);
    void AddExitEvent(GameObject obj);
    void AddButtonClick(GameObject button);
    bool Entry();
    bool Exit();
    bool Stay();
    bool ButtonClicked();
    void RecordSiblingIndex(GameObject obj);
    int GetOriginSiblingIndex();
    void SetSiblingBack(GameObject obj);
    void SetAsLastSibling(GameObject obj);
    void AlphaRaycastImage(GameObject obj, float a = 0.1f);
}
