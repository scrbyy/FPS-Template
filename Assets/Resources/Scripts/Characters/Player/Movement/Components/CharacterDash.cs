using Zenject;
using UnityEngine;

public class CharacterDash : MonoBehaviour
{
    [Header("Impulse Force")]
    [SerializeField] private float _airImpulse;
    [SerializeField] private float _groundImpulse;

    [Header("Settings")]
    [SerializeField] private float _cooldown;
    [SerializeField] private float _dashStaminaCost;

    [Header("References")]
    [SerializeField] private CharacterEngine _playerEngine;
    [SerializeField] private CharacterStamina _playerStamina;

    [Inject] private IInputProvider _inputProvider;
    [Inject] private IGroundChecker _groundCheck;

    private float _cooldownTimer;

    private void ExecuteDash()
    {
        Vector2 moveInput = _inputProvider.GetMoveVector();
        Vector3 dashDir;
        float impulse = _groundCheck.IsGrounded ? _groundImpulse : _airImpulse;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            dashDir = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)).normalized;
        } 
        else return;

        _playerEngine.AddForce(dashDir * impulse, ForceType.Impulse);

        _playerStamina.Decrease(_dashStaminaCost);
        _cooldownTimer = _cooldown;
    }

    private void TryPerformDash()
    {
        if (_cooldownTimer <= 0 && _playerStamina.IsEnoughStamina(_dashStaminaCost))
        {
            ExecuteDash();
        }
    }

    private void Update()
    {
        if (_cooldownTimer > 0)
        {
            _cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnEnable()
    {
        _inputProvider.OnDashPressed += TryPerformDash;
    }

    private void OnDisable()
    {
        _inputProvider.OnDashPressed -= TryPerformDash;
    }
}