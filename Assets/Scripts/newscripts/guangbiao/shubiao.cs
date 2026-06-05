using UnityEngine;
using UnityEngine.EventSystems;

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
        else if (EventSystem.current != null &&
                 EventSystem.current.IsPointerOverGameObject())
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