using scrbyy.FpsTemplate.Input.Interfaces;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace scrbyy.FpsTemplate.InputSystem
{
    public class NewInputSystemProvider : MonoBehaviour, IWeaponInputProvider, IMovementInputProvider, 
        ILookInputProvider, ILoadoutInputProvider, IInteractionInputProvider, IUIMenuProvider
    {
        public Vector2 MoveInput => _moveAction.ReadValue<Vector2>();

        public Vector2 LookInput => _lookAction.ReadValue<Vector2>();

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

        private InputAction _moveAction;
        private InputAction _jumpAction;
        private InputAction _lookAction;
        private InputAction _interactAction;
        private InputAction _sprintAction;
        private InputAction _fireAction;
        private InputAction _reloadAction;
        private InputAction _dashAction;
        private InputAction _nextWeaponAction;
        private InputAction _previousWeaponAction;
        private InputAction _debugMenuAction;
        private InputAction _pauseMenuAction;

        private void Awake()
        {
            _moveAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Move");
            _jumpAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Jump");
            _lookAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Look");
            _interactAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Interact");
            _sprintAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Sprint");
            _fireAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Attack");
            _reloadAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Reload");
            _dashAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("Dash");
            _nextWeaponAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("NextWeapon");
            _previousWeaponAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("PreviousWeapon");
            _debugMenuAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("ToggleDebug");
            _pauseMenuAction = UnityEngine.InputSystem.InputSystem.actions.FindAction("TogglePause");

            _jumpAction.performed += OnJump;
            _interactAction.performed += OnInteract;
            _reloadAction.performed += OnReload;
            _nextWeaponAction.performed += OnNextWeapon;
            _previousWeaponAction.performed += OnPreviousWeapon;

            _sprintAction.started += OnSprintStart;
            _sprintAction.canceled += OnSprintCancel;

            _dashAction.started += OnDashStart;

            _fireAction.started += OnShootStart;
            _fireAction.canceled += OnShootCancel;

            _debugMenuAction.performed += OnDebugMenu;
            _pauseMenuAction.performed += OnPauseMenu;
        }

        private void OnDestroy()
        {
            _jumpAction.performed -= OnJump;
            _interactAction.performed -= OnInteract;
            _reloadAction.performed -= OnReload;
            _nextWeaponAction.performed -= OnNextWeapon;
            _previousWeaponAction.performed -= OnPreviousWeapon;

            _sprintAction.started -= OnSprintStart;
            _sprintAction.canceled -= OnSprintCancel;

            _dashAction.started -= OnDashStart;

            _fireAction.started -= OnShootStart;
            _fireAction.canceled -= OnShootCancel;

            _debugMenuAction.performed -= OnDebugMenu;
            _pauseMenuAction.performed -= OnPauseMenu;
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