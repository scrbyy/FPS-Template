using UnityEngine;

public class CursorVisibility : MonoBehaviour
{
    public static void Hide()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public static void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
