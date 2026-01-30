using System.Collections;
using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionDistance;
    [SerializeField] private float maxClimbHeight;
    [SerializeField] private float minClimbHeight;
    [SerializeField] private LayerMask climbLayer;

    [Header("Procedural Timings")]
    [SerializeField] private float unitsPerSecond;
    [SerializeField] private AnimationCurve verticalCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    [SerializeField] private float horizontalDuration;
    [SerializeField] private float exitImpulse;

    [Header("Link Components")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private InputProvider inputProvider;

    private CharacterController _controller;
    private bool _isClimbing;

    private void Start() => _controller = GetComponent<CharacterController>();

    private void OnEnable() => inputProvider.OnJumpPerformed += HandleJumpInput;
    private void OnDisable() => inputProvider.OnJumpPerformed -= HandleJumpInput;

    private void HandleJumpInput()
    {
        if (!_controller.isGrounded && !_isClimbing)
        {
            TryClimb();
        }
    }

    private void TryClimb()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * 1.0f;
        if (Physics.Raycast(rayOrigin, transform.forward, out RaycastHit wallHit, detectionDistance, climbLayer))
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
        playerMovement.enabled = false;
        playerJump.enabled = false;

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

        playerMovement.enabled = true;
        playerJump.enabled = true;
        playerJump.ResetVerticalVelocity();

        playerMovement.AddForce(transform.forward * exitImpulse);

        _isClimbing = false;
    }
}