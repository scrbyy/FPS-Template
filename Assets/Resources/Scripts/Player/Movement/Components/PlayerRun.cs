using Zenject;
using UnityEngine;
using System.Collections;

public class PlayerRun : MonoBehaviour
{
    public event System.Action OnStartRunning;
    public event System.Action OnEndRunning;

    [SerializeField] private float _runSpeed;
    [SerializeField] private float _staminaCost;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;
    [SerializeField] private PlayerStamina _playerStamina;
    [SerializeField] private PlayerMovement _playerMovement;

    private Coroutine _cooldownCoroutine;

    [Inject] private IInputProvider _inputProvider;

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