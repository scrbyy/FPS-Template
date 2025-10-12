using UnityEngine;

public class CursorVisibility : MonoBehaviour
{
    private void Start()
    {
        Unshow();
    }
    public void Unshow()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void Show()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
