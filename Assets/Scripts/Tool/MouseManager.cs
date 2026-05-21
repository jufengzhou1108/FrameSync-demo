using UnityEngine;
using UnityEngine.InputSystem;

public static class CursorTool
{
    public static void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, new Vector2(32, 32), CursorMode.Auto);
    }
}