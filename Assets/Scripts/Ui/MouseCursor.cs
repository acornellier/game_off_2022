using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] Texture2D cursorDefault;
    [SerializeField] Texture2D cursorClick;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Mouse.current.leftButton.IsPressed())
        {
            Cursor.SetCursor(cursorClick,new Vector2(3,2),CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(cursorDefault, new Vector2(3, 2), CursorMode.Auto);
        }
    }
}
