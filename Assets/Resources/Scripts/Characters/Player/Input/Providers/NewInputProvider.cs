using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputProvider : MonoBehaviour, IInputProvider
{
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private InputAction sprintAction;
    private InputAction fireAction;
    private InputAction reloadAction;
    private InputAction dashAction;

    private InputAction selectNextWeaponAction;
    private InputAction selectPreviousWeaponAction;

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

    public Vector2 GetLookInput()
    {
        return lookAction.ReadValue<Vector2>();
    }

    public Vector2 GetMoveVector()
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
        dashAction = InputSystem.actions.FindAction("Dash");


        selectNextWeaponAction = InputSystem.actions.FindAction("SelectNextWeapon");
        selectPreviousWeaponAction = InputSystem.actions.FindAction("SelectPreviousWeapon");

        jumpAction.performed += ctx => OnJumpPerformed?.Invoke();
        interactAction.performed += ctx => OnInteractPerformed?.Invoke();
        fireAction.performed += ctx => OnShootTriggered?.Invoke();
        reloadAction.performed += ctx => OnReloadPerformed?.Invoke();

        sprintAction.canceled += ctx => OnSprintCanceled?.Invoke();
        dashAction.canceled += ctx => OnDashCanceled?.Invoke();

        selectNextWeaponAction.performed += ctx => OnNextWeaponSelect?.Invoke();
        selectPreviousWeaponAction.performed += ctx => OnPreviousWeaponSelect?.Invoke();
    }
    private void Update()
    {
        if (fireAction.IsPressed())
        {
            OnShootPressed?.Invoke();
        }

        if (sprintAction.IsPressed())
        {
            OnSprintPressed?.Invoke();
        }
        if (dashAction.IsPressed())
        {
            OnDashPressed?.Invoke();
        }
    }
}