using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerPhysics : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 velocity;
    private bool wasGrounded;

    public event System.Action OnLanded;
    public event System.Action OnLeftGround;

    public bool IsGrounded => controller.isGrounded;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        DetectGroundTransitions();
        ApplyGravity();
    }

    private void DetectGroundTransitions()
    {
        bool grounded = controller.isGrounded;

        if (grounded && !wasGrounded)
            OnLanded?.Invoke();
        else if (!grounded && wasGrounded)
            OnLeftGround?.Invoke();

        wasGrounded = grounded;
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0f)
            velocity.y = -2f;
        else
            velocity.y += Physics.gravity.y * Time.deltaTime;
    }

    public void Jump(float jumpForce)
    {
        if (controller.isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
    }

    public void Move(Vector3 motion)
    {
        Vector3 totalMove = new Vector3(motion.x, velocity.y, motion.z);
        controller.Move(totalMove * Time.deltaTime);
    }

    public Vector3 GetVelocity() => velocity;
}
