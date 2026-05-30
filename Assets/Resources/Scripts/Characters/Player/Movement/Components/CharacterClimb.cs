using Zenject;
using UnityEngine;
using System.Collections;

public class CharacterClimb : MonoBehaviour
{
    [Header("Limits")]
    [SerializeField] private float minClimbHeight;
    [SerializeField] private float maxClimbHeight;

    [Header("Detection Settings")]
    [SerializeField] private float detectionDistance;
    [SerializeField] private LayerMask climbLayer;

    [Header("Procedural Timings")]
    [SerializeField] private float unitsPerSecond;
    [SerializeField] private AnimationCurve verticalCurve;

    [Space]
    [SerializeField] private float horizontalDuration;
    [SerializeField] private float exitImpulse;

    [Header("References")]
    [SerializeField] private CharacterEngine _playerEngine;

    [Inject] private IGroundChecker _groundCheck;
    [Inject] private IInputProvider _inputProvider;

    private bool _isClimbing;
    private Vector3 _rayOrigin;

    private CharacterController _controller;

    private void Start()
    { 
        _controller = GetComponent<CharacterController>();
    }

    private void HandleJumpInput()
    {
        if (!_groundCheck.IsGrounded && !_isClimbing)
        {
            TryClimb();
        }
    }

    private void TryClimb()
    {
        _rayOrigin = transform.position;
        if (Physics.Raycast(_rayOrigin, transform.forward, out RaycastHit wallHit, detectionDistance, climbLayer))
        {
            Vector3 topCheckOrigin = wallHit.point + (transform.forward * 0.15f) + (Vector3.up * maxClimbHeight);

            if (Physics.Raycast(topCheckOrigin, Vector3.down, out RaycastHit topHit, maxClimbHeight + 0.2f, climbLayer))
            {
                float heightDifference = topHit.point.y - transform.position.y;

                if (heightDifference >= minClimbHeight && heightDifference <= maxClimbHeight)
                {
                    StopAllCoroutines();
                    StartCoroutine(PerformClimbRoutine(topHit.point, heightDifference));
                }
            }
        }
    }

    private IEnumerator PerformClimbRoutine(Vector3 targetSurfacePos, float height)
    {
        _isClimbing = true;
        _playerEngine.enabled = false;

        float bottomOffset = _controller.center.y - (_controller.height / 2f) - 0.04f;
        Vector3 startPos = transform.position;
        Vector3 peakPos = new Vector3(startPos.x, targetSurfacePos.y - bottomOffset + 0.05f, startPos.z);

        float dynamicVerticalDuration = (height / unitsPerSecond) + 0.1f;

        float elapsed = 0;
        while (elapsed < dynamicVerticalDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dynamicVerticalDuration;

            float curveT = verticalCurve.Evaluate(t);

            transform.position = Vector3.Lerp(startPos, peakPos, curveT);
            yield return null;
        }

        _playerEngine.enabled = true;

        _playerEngine.AddForce(transform.forward * exitImpulse, ForceType.Impulse);

        _isClimbing = false;
    }
    private void OnEnable() => _inputProvider.OnJumpPerformed += HandleJumpInput;
    private void OnDisable() => _inputProvider.OnJumpPerformed -= HandleJumpInput;
}