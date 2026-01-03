using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputProvider : InputProvider
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private InputAction sprintAction;
    private InputAction fireAction;
    private InputAction reloadAction;

    private InputAction selectNextWeaponAction;
    private InputAction selectPreviousWeaponAction;

    public override event Action OnJumpPerformed;
    public override event Action OnInteractPerformed;
    public override event Action OnReloadPerformed;
    public override event Action OnSprintStarted;
    public override event Action OnSprintCanceled;
    public override event Action OnShootTriggered;
    public override event Action OnShootPressed;
    public override event Action OnNextWeaponSelect;
    public override event Action OnPreviousWeaponSelect;

    public override Vector2 GetLookInput()
    {
        return lookAction.ReadValue<Vector2>();
    }

    public override Vector2 GetMoveVector()
    {
        return moveAction.ReadValue<Vector2>();
    }

    private void Awake()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        jumpAction = InputSystem.actions.FindAction("Jump");
        lookAction = InputSystem.actions.FindAction("Look");
        interactAction = InputSystem.actions.FindAction("Interact");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        fireAction = InputSystem.actions.FindAction("Fire");
        reloadAction = InputSystem.actions.FindAction("Reload");

        selectNextWeaponAction = InputSystem.actions.FindAction("SelectNextWeapon");
        selectPreviousWeaponAction = InputSystem.actions.FindAction("SelectPreviousWeapon");

        jumpAction.performed += ctx => OnJumpPerformed?.Invoke();
        interactAction.performed += ctx => OnInteractPerformed?.Invoke();
        fireAction.performed += ctx => OnShootTriggered?.Invoke();
        reloadAction.performed += ctx => OnReloadPerformed?.Invoke();

        sprintAction.started += ctx => OnSprintStarted?.Invoke();
        sprintAction.canceled += ctx => OnSprintCanceled?.Invoke();

        selectNextWeaponAction.performed += ctx => OnNextWeaponSelect?.Invoke();
        selectPreviousWeaponAction.performed += ctx => OnPreviousWeaponSelect?.Invoke();
    }
    private void Update()
    {
        if (fireAction.IsPressed())
        {
            OnShootPressed?.Invoke();
        }
    }
}