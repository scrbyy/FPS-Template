using Zenject;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class CharacterClimb : MonoBehaviour
{
    [Header("Climb Settings")]
    [SerializeField] private float _climbSpeed;
    [SerializeField] private AnimationCurve _verticalCurve;

    [Header("Limits")]
    [SerializeField] private float _minHeight;
    [SerializeField] private float _maxHeight;

    [Header("Detection Settings")]
    [SerializeField] private float _detectionDistance;
    [SerializeField] private LayerMask _climbLayer;

    [Space]
    [SerializeField] private float _minimalClimbTime;
    [SerializeField] private float _wallDepthOffset;

    [Header("Horizontal Impulse")]
    [SerializeField] private float _exitImpulseModifier;

    [Header("References")]
    [SerializeField] private Transform _origin;
    [SerializeField] private CharacterEngine _characterEngine;

    [Inject] private IGroundChecker _groundCheck;
    [Inject] private IInputProvider _inputProvider;

    private bool _isClimbing;
    private Vector3 _rayOrigin;
    private float _speedBeforeClimb;

    private Coroutine _climbCoroutine;

    private CharacterController _controller;

    private void Start()
    { 
        _controller = GetComponent<CharacterController>();
    }

    private void HandleJumpInput()
    {
        if (!_groundCheck.IsGrounded && !_isClimbing)
        {
            _speedBeforeClimb = _characterEngine.GetVelocity().z;
            CalculateWallTop();
        }
    }

    private void CalculateWallTop()
    {
        _rayOrigin = _origin.position;
        if (Physics.Raycast(_rayOrigin, transform.forward, out RaycastHit wallHit, _detectionDistance, _climbLayer))
        {
            Vector3 topCheckOrigin = wallHit.point + (transform.forward * _wallDepthOffset) + (Vector3.up * _maxHeight);

            if (Physics.Raycast(topCheckOrigin, Vector3.down, out RaycastHit topHit, _maxHeight , _climbLayer))
            {
                float heightDifference = topHit.point.y - transform.position.y;

                if (heightDifference >= _minHeight && heightDifference <= _maxHeight)
                {
                    if(_climbCoroutine != null) StopCoroutine(_climbCoroutine);
                    _climbCoroutine = StartCoroutine(PerformClimbRoutine(topHit.point, heightDifference));
                }
            }
        }
    }

    private IEnumerator PerformClimbRoutine(Vector3 targetSurfacePos, float height)
    {
        _isClimbing = true;
        _characterEngine.DisableMovement();

        float halfHeight = (_controller.height / 2f);
        float bottomOffset = _controller.center.y - halfHeight - _controller.skinWidth;

        Vector3 startPos = transform.position;
        Vector3 peakPos = new Vector3(startPos.x, targetSurfacePos.y - bottomOffset, startPos.z);

        float climbDuration = (height / _climbSpeed) + _minimalClimbTime;

        float elapsed = 0;
        while (elapsed < climbDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / climbDuration;

            float curveT = _verticalCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, peakPos, curveT);
            yield return null;
        }

        _characterEngine.EnableMovement();

        _characterEngine.AddForce(transform.forward * (_speedBeforeClimb * _exitImpulseModifier), ForceType.Impulse);

        _isClimbing = false;
    }

    private void OnEnable() => _inputProvider.OnJumpPerformed += HandleJumpInput;

    private void OnDisable() => _inputProvider.OnJumpPerformed -= HandleJumpInput;
}