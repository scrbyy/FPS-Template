using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStamina))]
[RequireComponent(typeof(PlayerEngine))]

public class PlayerRun : MonoBehaviour
{
    public event System.Action OnStartRunning;
    public event System.Action OnEndRunning;

    [Header("Main")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float staminaCost;

    [Header("Link Components")]
    [SerializeField] private InputProvider inputProvider;

    private PlayerStamina _playerStamina;
    private PlayerMovement _playerMovement;
    private PlayerEngine _playerEngine;
    private CharacterController _characterController;


    private void Start()
    {
        _playerStamina = GetComponent<PlayerStamina>();
        _playerMovement = GetComponent<PlayerMovement>();
        _characterController = GetComponent<CharacterController>();
        _playerEngine = GetComponent<PlayerEngine>();
    }

    private void OnEnable()
    {
        inputProvider.OnSprintPressed += () => TryRun();
        inputProvider.OnSprintCanceled += () => CancelRun();
    }

    private void OnDisable()
    {
        inputProvider.OnSprintPressed -= () => TryRun();
        inputProvider.OnSprintCanceled -= () => CancelRun();
    }

    private void TryRun()
    {
        if (_characterController.isGrounded)
        {
            if (_playerStamina.IsEnoughStamina(staminaCost) == false)
            {
                _playerStamina.StayExhausted();
                CancelRun();
            }
            else if (_playerEngine.IsMoving())
            {
                _playerMovement.SetTargetSpeed(runSpeed);
                _playerStamina.ReduceStamina(staminaCost);
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