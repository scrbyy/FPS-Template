using UnityEngine;
using Zenject;

public class CharacterStateHandler : MonoBehaviour 
{
    public float Speed => _finalSpeed;

    [SerializeField] private CharacterMovementData _data;

    [SerializeField] private CharacterRun _characterRun;

    private float _currentSpeed;
    private float _finalSpeed;

    [Inject] private IGroundChecker _groundCheck;

    private void Start()
    {
        SetWalkSpeed();
    }
    public void SetSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _currentSpeed = newSpeed;

        _finalSpeed = _currentSpeed;
    }

    public void SetSpeedModifier(float modifier)
    {
        if(modifier < 0) return;
        
        _finalSpeed = _currentSpeed * modifier;
    }

    private void OnLandedReset()
    {
        _groundCheck.OnGrounded -= OnLandedReset;
        _currentSpeed = _data.WalkSpeed;
    }

    private void SetRunSpeed()
    {
        SetSpeed(_data.RunSpeed);
    }

    private void SetWalkSpeed()
    {
        SetSpeed(_data.WalkSpeed);
    }

    private void OnEnable()
    {
        _characterRun.OnStartRunning += SetRunSpeed;
        _characterRun.OnEndRunning += SetWalkSpeed;
    }

    private void OnDisable()
    {
        _characterRun.OnStartRunning -= SetRunSpeed;
        _characterRun.OnEndRunning -= SetWalkSpeed;
    }
}