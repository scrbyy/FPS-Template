using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterSpeed : MonoBehaviour 
{
    public float Speed => _finalSpeed;

    [Header("Modifier Clamping")]
    [SerializeField] private float _maxModifier;
    [SerializeField] private float _minModifier;

    [Header("References")]
    [SerializeField] private CharacterMovementData _data;
    [SerializeField] private CharacterRun _characterRun;

    private List<ISpeedModifier> _modifiers = new List<ISpeedModifier>();

    private float _currentSpeed;
    private float _finalSpeed;

    [Inject] private IGroundChecker _groundCheck;

    public void AddModifier(ISpeedModifier modifier)
    {
        if (!_modifiers.Contains(modifier))
        {
            _modifiers.Add(modifier);
            UpdateFinalSpeed();
        }
    }

    public void RemoveModifier(ISpeedModifier modifier)
    {
        if (_modifiers.Contains(modifier))
        {
            _modifiers.Remove(modifier);
            UpdateFinalSpeed();
        }
        else Debug.Log("Assigned Modifier not found");
    }

    private void UpdateFinalSpeed()
    {
        float totalReduction = 0f;
        foreach (var mod in _modifiers)
            totalReduction += (1.0f - mod.SpeedMultiplier);

        float finalMultiplier = Mathf.Max(_minModifier, _maxModifier - totalReduction);

        _finalSpeed = _currentSpeed * finalMultiplier;
    }

    private void Start()
    {
        SetWalkSpeed();
    }

    public void SetSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _currentSpeed = newSpeed;

        UpdateFinalSpeed();
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