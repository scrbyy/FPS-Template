using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerEngine))]
[RequireComponent(typeof(PlayerMovement))]

public class PlayerRun : MonoBehaviour
{
    public event System.Action OnStartRunning;
    public event System.Action OnEndRunning;

    [Header("Main")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _staminaCost;

    [Header("References")]
    [SerializeField] private InputProvider _inputProvider;
    [SerializeField] private PlayerStamina _playerStamina;

    private PlayerEngine _playerEngine;
    private PlayerMovement _playerMovement;

    private Coroutine _cooldownCoroutine;

    private void TryRun()
    {
        if (_playerEngine.isGrounded())
        {
            if (_playerStamina.IsEnoughStamina(_staminaCost))
            {
                if (_playerEngine.IsMoving() && _cooldownCoroutine == null)
                {
                    _cooldownCoroutine = StartCoroutine(ReducingDelay());
                    _playerMovement.SetTargetSpeed(_runSpeed);
                    OnStartRunning?.Invoke();
                }
            }
            else CancelRun();
        }
    }
    private void CancelRun() 
    {
        _playerMovement.ResetSpeed(); 
        StopCoroutine(ReducingDelay());
        _cooldownCoroutine = null;
        OnEndRunning?.Invoke();
    }

    private IEnumerator ReducingDelay()
    {
        yield return new WaitForFixedUpdate();
        _playerStamina.Decrease(_staminaCost);
        _cooldownCoroutine = null;
    }

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
        _playerEngine = GetComponent<PlayerEngine>();
    }

    private void OnEnable()
    {
        _inputProvider.OnSprintPressed += () => TryRun();
        _inputProvider.OnSprintCanceled += () => CancelRun();
    }

    private void OnDisable()
    {
        _inputProvider.OnSprintPressed -= () => TryRun();
        _inputProvider.OnSprintCanceled -= () => CancelRun();
    }
}