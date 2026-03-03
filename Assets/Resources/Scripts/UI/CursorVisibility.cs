using UnityEngine;

public class CursorVisibility : MonoBehaviour
{
    private void Start()
    {
        Hide();
    }

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
