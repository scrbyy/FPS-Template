using System.Collections.Generic;
using UnityEngine;

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

    private float _baseSpeed;
    private float _finalSpeed;

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

        float finalMultiplier = 1.0f - totalReduction;
        finalMultiplier = Mathf.Clamp(finalMultiplier, _minModifier, _maxModifier);

        _finalSpeed = _baseSpeed * finalMultiplier;
    }

    private void Start()
    {
        SetWalkSpeed();
    }

    public void SetSpeed(float newSpeed)
    {
        if (newSpeed < 0) return;

        _baseSpeed = newSpeed;

        UpdateFinalSpeed();
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
        _characterRun.OnRunStarted += SetRunSpeed;
        _characterRun.OnRunEnded += SetWalkSpeed;
    }

    private void OnDisable()
    {
        _characterRun.OnRunStarted -= SetRunSpeed;
        _characterRun.OnRunEnded -= SetWalkSpeed;
    }
}