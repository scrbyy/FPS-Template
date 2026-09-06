using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class NewInputProvider : MonoBehaviour,
    IWeaponInputProvider,
    IMovementInputProvider,
    ILookInputProvider,
    ILoadoutInputProvider,
    IInteractionInputProvider
{
    private InputSettings _inputSettings;

    public Vector2 MoveInput => _inputSettings.Player.Move.ReadValue<Vector2>();
    public Vector2 LookInput => _inputSettings.Player.Look.ReadValue<Vector2>();

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

    [Inject]
    public void Construct(InputSettings inputSettings)
    {
        _inputSettings = inputSettings;
    }

    private void Awake()
    {
        _inputSettings.Player.Jump.performed += OnJump;
        _inputSettings.Player.Interact.performed += OnInteract;
        _inputSettings.Player.Reload.performed += OnReload;
        _inputSettings.Player.SelectNextWeapon.performed += OnNextWeapon;
        _inputSettings.Player.SelectPreviousWeapon.performed += OnPreviousWeapon;

        _inputSettings.Player.Sprint.started += OnSprintStart;
        _inputSettings.Player.Sprint.canceled += OnSprintCancel;

        _inputSettings.Player.Dash.started += OnDashStart;

        _inputSettings.Player.Fire.started += OnShootStart;
        _inputSettings.Player.Fire.canceled += OnShootCancel;
    }

    private void OnDestroy()
    {
        if (_inputSettings == null) return;

        _inputSettings.Player.Jump.performed -= OnJump;
        _inputSettings.Player.Interact.performed -= OnInteract;
        _inputSettings.Player.Reload.performed -= OnReload;
        _inputSettings.Player.SelectNextWeapon.performed -= OnNextWeapon;
        _inputSettings.Player.SelectPreviousWeapon.performed -= OnPreviousWeapon;

        _inputSettings.Player.Sprint.started -= OnSprintStart;
        _inputSettings.Player.Sprint.canceled -= OnSprintCancel;

        _inputSettings.Player.Dash.started -= OnDashStart;

        _inputSettings.Player.Fire.started -= OnShootStart;
        _inputSettings.Player.Fire.canceled -= OnShootCancel;
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