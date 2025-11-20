using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerRun : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float runAccelerationRate;
    [SerializeField] private float runDecelerationRate;

    [SerializeField] private InputProvider inputProvider;

    private PlayerMovement _playerMovement;

    private void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        inputProvider.OnSprintStarted += () => _playerMovement.SetMaxSpeed(runSpeed);
        inputProvider.OnSprintCanceled += () => _playerMovement.ResetMaxSpeed();
    }

    private void OnDisable()
    {
        inputProvider.OnSprintStarted -= () => _playerMovement.SetMaxSpeed(runSpeed);
        inputProvider.OnSprintCanceled -= () => _playerMovement.ResetMaxSpeed();
    }
}