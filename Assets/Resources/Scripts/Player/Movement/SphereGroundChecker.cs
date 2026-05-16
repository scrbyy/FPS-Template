using System;
using UnityEngine;

public class SphereGroundChecker : MonoBehaviour, IGroundChecker
{
    [Header("Sphere Settings")]
    [SerializeField] private Transform _checkPoint;
    [SerializeField] private float _radius = 0.3f;
    [SerializeField] private float _castDistance = 0.1f;
    [SerializeField] private LayerMask _groundLayer;

    public event Action OnGrounded;

    private bool _isGrounded;
    private bool _wasGrounded;

    public bool IsGrounded => _isGrounded;

    private void Update()
    {
        Debug.Log(_isGrounded);

        Vector3 checkPosition = _checkPoint.position + (Vector3.down * _castDistance);

        _isGrounded = Physics.CheckSphere(checkPosition, _radius, _groundLayer);

        if (_isGrounded && !_wasGrounded)
        {
            OnGrounded?.Invoke();
        }

        _wasGrounded = _isGrounded;
    }

    private void OnDrawGizmosSelected()
    {
        if (_checkPoint == null) return;

        Gizmos.color = _isGrounded ? Color.green : Color.red;
        Gizmos.DrawWireSphere(_checkPoint.position, _radius);

        Gizmos.color = Color.cyan;
        Vector3 endPosition = _checkPoint.position + (Vector3.down * _castDistance);
        Gizmos.DrawWireSphere(endPosition, _radius);
    }
}