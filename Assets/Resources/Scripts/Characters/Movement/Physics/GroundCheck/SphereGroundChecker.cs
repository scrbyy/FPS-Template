using System;
using UnityEngine;

public class SphereGroundChecker : MonoBehaviour, IGroundChecker
{
    public event Action OnGrounded;

    public bool IsGrounded => _isGrounded;

    [Header("Sphere Settings")]
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _radius;
    [SerializeField] private float _castDistance;
    [SerializeField] private LayerMask _groundLayer;

    private bool _isGrounded;
    private bool _wasGrounded;

    private void Update()
    {
        Vector3 checkPosition = _checkPoint.position + (Vector3.down * _castDistance);

        _isGrounded = Physics.CheckSphere(checkPosition, _radius, _groundLayer);

        if (_isGrounded && !_wasGrounded)
        {
            OnGrounded?.Invoke();
        }

        _wasGrounded = _isGrounded;
    }
}