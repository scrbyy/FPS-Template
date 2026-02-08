using UnityEngine;

public class PlayerDash : MonoBehaviour
{
    [Header("Dash Settings")]
    [SerializeField] private float dashImpulse;
    [SerializeField] private float dashStaminaCost;
    [SerializeField] private float cooldown;

    [Header("Link Components")]
    [SerializeField] private PlayerEngine playerEngine;
    [SerializeField] private PlayerStamina playerStamina;
    [SerializeField] private InputProvider inputProvider;

    private float _cooldownTimer;

    private void OnEnable()
    {
        inputProvider.OnDashPressed += TryPerformDash;
    }

    private void OnDisable()
    {
        inputProvider.OnDashPressed -= TryPerformDash;
    }

    private void Update()
    {
        if (_cooldownTimer > 0)
            _cooldownTimer -= Time.deltaTime;
    }

    private void TryPerformDash()
    {
        if (_cooldownTimer <= 0 && playerStamina.IsEnoughStamina(dashStaminaCost))
        {
            ExecuteDash();
        }
    }

    private void ExecuteDash()
    {
        Vector2 moveInput = inputProvider.GetMoveVector();
        Vector3 dashDir;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            dashDir = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y)).normalized;
        }
        else
        {
            dashDir = transform.forward;
        }

        playerEngine.AddForce(dashDir * dashImpulse);

        playerStamina.ReduceStamina(dashStaminaCost);
        _cooldownTimer = cooldown;
    }
}