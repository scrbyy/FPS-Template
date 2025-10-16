using UnityEngine;

public abstract class InputProvider : MonoBehaviour
{
    public abstract Vector2 GetKeyboardInput();
    public abstract Vector2 GetMouseInput();
    public abstract bool isJumpButtonDown();
    public abstract bool isInteractButtonDown();
    public abstract bool isSprintButtonPressed();
}
