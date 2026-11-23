using UnityEngine;
using Zenject;

using scrbyy.FpsTemplate.InputSystem.Interfaces;
using scrbyy.FpsTemplate.Services;
using scrbyy.FpsTemplate.Characters.Stats;
using scrbyy.FpsTemplate.Characters.Player.Movement.Collisions.GroundChecker;

namespace scrbyy.FpsTemplate.Characters.Player.Movement.Abilities
{
    public class CharacterJump : MonoBehaviour
    {
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _staminaCost;

        [Header("Buffer Settings")]
        [SerializeField] private float _inputBufferDuration;
        [SerializeField] private float _coyoteTimeDuration;

        [Header("References")]
        [SerializeField] private CharacterEngine _characterEngine;
        [SerializeField] private CharacterStamina _characterStamina;

        [Inject] private IMovementInputProvider _inputProvider;
        [Inject] private IGroundChecker _groundCheck;

        private Buffer _inputBuffer;
        private Buffer _coyoteTimer;

        private void Awake()
        {
            _inputBuffer = new Buffer(_inputBufferDuration);
            _coyoteTimer = new Buffer(_coyoteTimeDuration);
        }

        private void OnEnable()
        {
            if (_inputProvider != null)
                _inputProvider.OnJumpStarted += BufferJumpInput;
        }

        private void OnDisable()
        {
            if (_inputProvider != null)
                _inputProvider.OnJumpStarted -= BufferJumpInput;
        }

        private void Update()
        {
            if (_groundCheck.IsGrounded)
            {
                _coyoteTimer.Set();
            }

            TryExecuteJump();
        }

        private void BufferJumpInput()
        {
            _inputBuffer.Set();
        }

        private void TryExecuteJump()
        {
            if (_inputBuffer.Has() && _coyoteTimer.Has())
            {
                if (_characterStamina.IsEnoughStamina(_staminaCost))
                {
                    ExecuteJump();
                }
            }
        }

        private void ExecuteJump()
        {
            _characterEngine.Jump(_jumpForce);
            _characterStamina.Decrease(_staminaCost);

            _inputBuffer.Reset();
            _coyoteTimer.Reset();
        }
    }
}