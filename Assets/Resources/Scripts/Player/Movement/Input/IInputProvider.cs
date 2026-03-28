using System;
using UnityEngine;

public interface IInputProvider
{
    public abstract Vector2 GetMoveVector();
    public abstract Vector2 GetLookInput();

    public abstract event Action OnJumpPerformed;
    public abstract event Action OnInteractPerformed;
    public abstract event Action OnReloadPerformed;

    public abstract event Action OnSprintCanceled;
    public abstract event Action OnSprintPressed;

    public abstract event Action OnShootTriggered;
    public abstract event Action OnShootPressed;
    
    public abstract event Action OnNextWeaponSelect;
    public abstract event Action OnPreviousWeaponSelect;

    public abstract event Action OnDashCanceled;
    public abstract event Action OnDashPressed;
}
