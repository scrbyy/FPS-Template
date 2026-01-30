using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(CharacterController))]

public class PlayerRun : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float staminaCost;

    [Header("Link Components")]
    [SerializeField] private InputProvider inputProvider;

    private PlayerStamina _playerStamina;
    private PlayerMovement _playerMovement;
    private CharacterController _characterController;


    private void Start()
    {
        _playerStamina = GetComponent<PlayerStamina>();
        _playerMovement = GetComponent<PlayerMovement>();
        _characterController = GetComponent<CharacterController>();
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
        if (_characterController.isGrounded && _playerStamina.IsEnoughStamina(staminaCost))
        {
            _playerMovement.SetTargetSpeed(runSpeed);
            _playerStamina.ReduceStamina(staminaCost);
        }
    }
    private void CancelRun() 
    {
        _playerMovement.ResetSpeed(); 
    }
}