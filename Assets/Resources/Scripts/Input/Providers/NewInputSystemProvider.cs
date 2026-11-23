using System;
using UnityEngine;
using UnityEngine.InputSystem;

using scrbyy.FpsTemplate.InputSystem.Interfaces;
using Zenject;

namespace scrbyy.FpsTemplate.InputSystem
{
    public class NewInputSystemProvider : MonoBehaviour, IWeaponInputProvider, IMovementInputProvider, 
        ILookInputProvider, ILoadoutInputProvider, IInteractionInputProvider, IUIMenuProvider
    {
        public Vector2 MoveInput => _inputActions?.Player.Move.ReadValue<Vector2>() ?? Vector2.zero;

        public Vector2 LookInput => _inputActions?.Player.Look.ReadValue<Vector2>() ?? Vector2.zero;

        public event Action OnShootReleased;
        public event Action OnShootStarted;
        public event Action OnReloadStarted;

        public event Action OnJumpStarted;
        public event Action OnSprintStarted;
        public event Action OnSprintReleased;
        public event Action OnDashStarted;

        public event Action OnNextWeaponSelected;
        public event Action OnPreviousWeaponSelected;

        public event Action OnInteractStarted;

        public event Action OnDebugMenuToggled;
        public event Action OnPauseMenuToggled;

        [Inject] private InputActionsAsset _inputActions;

        private void Awake()
        {
            _inputActions.Player.Jump.performed += OnJump;
            _inputActions.Player.Interact.performed += OnInteract;
            _inputActions.Player.Reload.performed += OnReload;
            _inputActions.Player.NextWeapon.performed += OnNextWeapon;
            _inputActions.Player.PreviousWeapon.performed += OnPreviousWeapon;

            _inputActions.Player.Sprint.started += OnSprintStart;
            _inputActions.Player.Sprint.canceled += OnSprintCancel;

            _inputActions.Player.Dash.started += OnDashStart;

            _inputActions.Player.Fire.started += OnShootStart;
            _inputActions.Player.Fire.canceled += OnShootCancel;

            _inputActions.Player.ToggleDebug.performed += OnDebugMenu;
            _inputActions.Player.TogglePause.performed += OnPauseMenu;
        }

        private void OnDestroy()
        {
            _inputActions.Player.Jump.performed -= OnJump;
            _inputActions.Player.Interact.performed -= OnInteract;
            _inputActions.Player.Reload.performed -= OnReload;
            _inputActions.Player.NextWeapon.performed -= OnNextWeapon;
            _inputActions.Player.PreviousWeapon.performed -= OnPreviousWeapon;

            _inputActions.Player.Sprint.started -= OnSprintStart;
            _inputActions.Player.Sprint.canceled -= OnSprintCancel;

            _inputActions.Player.Dash.started -= OnDashStart;

            _inputActions.Player.Fire.started -= OnShootStart;
            _inputActions.Player.Fire.canceled -= OnShootCancel;

            _inputActions.Player.ToggleDebug.performed -= OnDebugMenu;
            _inputActions.Player.TogglePause.performed -= OnPauseMenu;
        }

        private void OnJump(InputAction.CallbackContext ctx) => OnJumpStarted?.Invoke();

        private void OnInteract(InputAction.CallbackContext ctx) => OnInteractStarted?.Invoke();

        private void OnReload(InputAction.CallbackContext ctx) => OnReloadStarted?.Invoke();

        private void OnNextWeapon(InputAction.CallbackContext ctx) => OnNextWeaponSelected?.Invoke();

        private void OnPreviousWeapon(InputAction.CallbackContext ctx) => OnPreviousWeaponSelected?.Invoke();

        private void OnSprintStart(InputAction.CallbackContext ctx) => OnSprintStarted?.Invoke();

        private void OnSprintCancel(InputAction.CallbackContext ctx) => OnSprintReleased?.Invoke();

        private void OnDashStart(InputAction.CallbackContext ctx) => OnDashStarted?.Invoke();

        private void OnShootStart(InputAction.CallbackContext ctx) => OnShootStarted?.Invoke();

        private void OnShootCancel(InputAction.CallbackContext ctx) => OnShootReleased?.Invoke();

        private void OnDebugMenu(InputAction.CallbackContext ctx) => OnDebugMenuToggled?.Invoke();

        private void OnPauseMenu(InputAction.CallbackContext ctx) => OnPauseMenuToggled?.Invoke();
    }
}