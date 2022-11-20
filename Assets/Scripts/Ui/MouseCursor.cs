using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorDefault;
    [SerializeField] Texture2D cursorClick;

    void Update()
    {
        Cursor.SetCursor(
            Mouse.current.leftButton.IsPressed() ? cursorClick : cursorDefault,
            new Vector2(3, 2),
            CursorMode.Auto
        );
    }
}