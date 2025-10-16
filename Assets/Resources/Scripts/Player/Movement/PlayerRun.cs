using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerSpeedController : MonoBehaviour
{
    [SerializeField] private float runSpeed;
    [SerializeField] private float speedChangeDuration;
    [SerializeField] private InputProvider inputProvider;

    private PlayerMovement movement;
    private PlayerPhysics _playerPhysics;
    private Coroutine speedCoroutine;

    private float walkSpeed;
    private float targetSpeed;
    private bool sprintPressed;
    private bool canChangeSpeed = true;

    private void Start()
    {
        movement = GetComponent<PlayerMovement>();
        _playerPhysics = GetComponent<PlayerPhysics>();

        walkSpeed = movement.GetMoveSpeed();
        targetSpeed = walkSpeed;

        _playerPhysics.OnLeftGround += HandleLeftGround;
        _playerPhysics.OnLanded += HandleLanded;
    }

    private void Update()
    {
        bool sprint = inputProvider.isSprintButtonPressed();

        if (sprint != sprintPressed)
        {
            sprintPressed = sprint;

            if (canChangeSpeed)
                SetTargetSpeed(sprint ? runSpeed : walkSpeed);
            else
                targetSpeed = sprint ? runSpeed : walkSpeed;
        }
    }

    private void HandleLeftGround() => canChangeSpeed = false;

    private void HandleLanded() => StartCoroutine(DelayedGroundSpeedChange());

    private IEnumerator DelayedGroundSpeedChange()
    {
        yield return new WaitForSeconds(0.1f);
        canChangeSpeed = true;
        SetTargetSpeed(targetSpeed);
    }

    private void SetTargetSpeed(float newSpeed)
    {
        if (speedCoroutine != null)
            StopCoroutine(speedCoroutine);

        speedCoroutine = StartCoroutine(SmoothChangeSpeed(newSpeed, speedChangeDuration));
    }

    private IEnumerator SmoothChangeSpeed(float targetSpeed, float duration)
    {
        float startSpeed = movement.GetMoveSpeed();
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newSpeed = Mathf.Lerp(startSpeed, targetSpeed, t);
            movement.SetMoveSpeed(newSpeed);
            yield return null;
        }

        movement.SetMoveSpeed(targetSpeed);
    }
}
