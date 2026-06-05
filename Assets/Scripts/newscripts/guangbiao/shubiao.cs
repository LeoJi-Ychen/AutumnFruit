using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public class CursorManager : MonoBehaviour
{
    public Texture2D normalCursor;
    public Texture2D hoverCursor;
    public Texture2D pressCursor;

    private Texture2D currentCursor;

    void Start()
    {
        SetCursor(normalCursor);
    }

    void Update()
    {
        Texture2D targetCursor;

        if (Input.GetMouseButton(0))
        {
            targetCursor = pressCursor;
        }
        else if (IsHoveringInteractable())
        {
            targetCursor = hoverCursor;
        }
        else
        {
            targetCursor = normalCursor;
        }

        if (currentCursor != targetCursor)
        {
            SetCursor(targetCursor);
        }
    }

    bool IsHoveringInteractable()
    {
        if (EventSystem.current == null)
            return false;

        PointerEventData pointerData =
            new PointerEventData(EventSystem.current);

        pointerData.position = Input.mousePosition;

        List<RaycastResult> results =
            new List<RaycastResult>();

        EventSystem.current.RaycastAll(pointerData, results);

        foreach (RaycastResult result in results)
        {
            GameObject obj = result.gameObject;

            // Unity Button
            if (obj.GetComponent<Button>() != null)
                return true;

            // 你的拼图
            if (obj.GetComponent<PuzzlePiece>() != null)
                return true;

            // 你的 CloudClick
            if (obj.GetComponent<CloudClick>() != null)
                return true;
        }

        return false;
    }

    void SetCursor(Texture2D tex)
    {
        currentCursor = tex;

        Cursor.SetCursor(
            tex,
            Vector2.zero,
            CursorMode.Auto
        );
    }
}