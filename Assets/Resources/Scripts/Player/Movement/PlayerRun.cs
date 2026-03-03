using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerStamina))]
[RequireComponent(typeof(PlayerEngine))]

public class PlayerRun : MonoBehaviour
{
    public event System.Action OnStartRunning;
    public event System.Action OnEndRunning;

    [Header("Main")]
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _staminaCost;

    [Header("References")]
    [SerializeField] private InputProvider _inputProvider;

    private PlayerStamina _playerStamina;
    private PlayerMovement _playerMovement;
    private PlayerEngine _playerEngine;


    private void Start()
    {
        _playerStamina = GetComponent<PlayerStamina>();
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

    private void TryRun()
    {
        if (_playerEngine.isGrounded())
        {
            if (_playerStamina.IsEnoughStamina(_staminaCost) == false)
            {
                _playerStamina.StayExhausted();
                CancelRun();
            }
            else if (_playerEngine.IsMoving())
            {
                _playerMovement.SetTargetSpeed(_runSpeed);
                _playerStamina.ReduceStamina(_staminaCost);
                OnStartRunning?.Invoke();
            }
        }
    }
    private void CancelRun() 
    {
        _playerMovement.ResetSpeed(); 
        OnEndRunning?.Invoke();
    }
}