using UnityEngine;

public abstract class InputProvider : MonoBehaviour
{
    public abstract Vector2 GetKeyboardInput();
    public abstract Vector2 GetMouseInput();
    public abstract bool isJumpButtonPressed();
    public abstract bool isInteractButtonPressed();
}
