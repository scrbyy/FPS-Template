using System;
using UnityEngine;

public abstract class InputProvider : MonoBehaviour
{
    public abstract Vector2 GetMoveVector();
    public abstract Vector2 GetLookInput();

    public abstract event Action OnJumpPerformed;
    public abstract event Action OnInteractPerformed;
    public abstract event Action OnReloadPerformed;

    public abstract event Action OnSprintStarted;
    public abstract event Action OnSprintCanceled;

    public abstract event Action OnShootTriggered;
    public abstract event Action OnShootPressed;
}
