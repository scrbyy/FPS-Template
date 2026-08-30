using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewInputProvider : MonoBehaviour, IWeaponInputProvider, IMovementInputProvider, ILookInputProvider, ILoadoutInputProvider, IInteractionInputProvider
{
    public Vector2 MoveInput => moveAction.ReadValue<Vector2>();

    public Vector2 LookInput => lookAction.ReadValue<Vector2>();

    public event Action OnShootReleased;
    public event Action OnShootStarted;
    public event Action OnReloadStarted;

    public event Action OnJumpStarted;
    public event Action OnSprintStarted;
    public event Action OnSprintReleased;
    public event Action OnDashStarted;

    public event Action OnNextWeaponSelect;
    public event Action OnPreviousWeaponSelect;

    public event Action OnInteractStarted;

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

        jumpAction.performed += OnJump;
        interactAction.performed += OnInteract;
        reloadAction.performed += OnReload;
        selectNextWeaponAction.performed += OnNextWeapon;
        selectPreviousWeaponAction.performed += OnPreviousWeapon;

        sprintAction.started += OnSprintStart;
        sprintAction.canceled += OnSprintCancel;

        dashAction.started += OnDashStart;

        fireAction.started += OnShootStart;
        fireAction.canceled += OnShootCancel;
    }

    private void OnDestroy()
    {
        jumpAction.performed -= OnJump;
        interactAction.performed -= OnInteract;
        reloadAction.performed -= OnReload;
        selectNextWeaponAction.performed -= OnNextWeapon;
        selectPreviousWeaponAction.performed -= OnPreviousWeapon;

        sprintAction.started -= OnSprintStart;
        sprintAction.canceled -= OnSprintCancel;

        dashAction.started -= OnDashStart;

        fireAction.started -= OnShootStart;
        fireAction.canceled -= OnShootCancel;
    }

    private void OnJump(InputAction.CallbackContext ctx) => OnJumpStarted?.Invoke();

    private void OnInteract(InputAction.CallbackContext ctx) => OnInteractStarted?.Invoke();

    private void OnReload(InputAction.CallbackContext ctx) => OnReloadStarted?.Invoke();

    private void OnNextWeapon(InputAction.CallbackContext ctx) => OnNextWeaponSelect?.Invoke();

    private void OnPreviousWeapon(InputAction.CallbackContext ctx) => OnPreviousWeaponSelect?.Invoke();

    private void OnSprintStart(InputAction.CallbackContext ctx) => OnSprintStarted?.Invoke();

    private void OnSprintCancel(InputAction.CallbackContext ctx) => OnSprintReleased?.Invoke();

    private void OnDashStart(InputAction.CallbackContext ctx) => OnDashStarted?.Invoke();

    private void OnShootStart(InputAction.CallbackContext ctx) => OnShootStarted?.Invoke();

    private void OnShootCancel(InputAction.CallbackContext ctx) => OnShootReleased?.Invoke();
}