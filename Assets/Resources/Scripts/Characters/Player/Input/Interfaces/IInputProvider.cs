using System;
using UnityEngine;

public interface IInputProvider
{
    public abstract Vector2 GetMoveVector();
    public abstract Vector2 GetLookInput();

    public event Action OnJumpPerformed;
    public event Action OnInteractPerformed;
    public event Action OnReloadPerformed;

    public event Action OnSprintCanceled;
    public event Action OnSprintPressed;

    public event Action OnShootTriggered;
    public event Action OnShootPressed;
    
    public event Action OnNextWeaponSelect;
    public event Action OnPreviousWeaponSelect;

    public event Action OnDashCanceled;
    public event Action OnDashPressed;
}
