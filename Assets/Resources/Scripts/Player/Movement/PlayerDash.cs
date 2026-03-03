using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Impulse Force")]
    [SerializeField] private float _groundImpulse;
    [SerializeField] private float _airImpulse;

    [Header("Settings")]
    [SerializeField] private float _dashStaminaCost;
    [SerializeField] private float _cooldown;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;
    [SerializeField] private PlayerStamina _playerStamina;
    [SerializeField] private InputProvider _inputProvider;

    private float _cooldownTimer;

    private void OnEnable()
    {
        _inputProvider.OnDashPressed += TryPerformDash;
    }

    private void OnDisable()
    {
        _inputProvider.OnDashPressed -= TryPerformDash;
    }

    private void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }

    private void TryPerformDash()
    {
        if (_cooldownTimer <= 0 && _playerStamina.IsEnoughStamina(_dashStaminaCost))
        {
            ExecuteDash();
        }
    }

    private void ExecuteDash()
    {
        Vector2 moveInput = _inputProvider.GetMoveVector();
        Vector3 dashDir;
        float impulse = _playerEngine.isGrounded() ? _groundImpulse : _airImpulse;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            dashDir = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)).normalized;
        }
        else
        {
            dashDir = transform.forward;
        }

        _playerEngine.AddForce(dashDir * impulse, ForceType.Impulse);

        _playerStamina.ReduceStamina(_dashStaminaCost);
        _cooldownTimer = _cooldown;
    }
}