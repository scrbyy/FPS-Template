using UnityEngine;

public class LandingPhysics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerEngine playerEngine;

    [Header("Spring Settings")]
    [SerializeField] private float stiffness;
    [SerializeField] private float damping;
    [SerializeField] private float mass;

    [Header("Impact Force")]
    [SerializeField] private float forceMultiplier;
    [SerializeField] private float maxForce;

    private Vector3 _currentPosition;
    private Vector3 _velocity;
    private Vector3 _initialLocalPosition;
    private float _lastVerticalVelocity;

    private void Awake() => _initialLocalPosition = transform.localPosition;

    private void OnEnable()
    {
        if (playerJump != null) playerEngine.OnLanded += ApplyLandingForce;
    }

    private void OnDisable()
    {
        if (playerJump != null) playerEngine.OnLanded -= ApplyLandingForce;
    }

    private void Update()
    {
        float currentYVel = playerEngine.GetVelocity().y;
        if (currentYVel < -0.1f)
            _lastVerticalVelocity = currentYVel;
    }

    private void LateUpdate()
    {
        Vector3 force = -stiffness * _currentPosition - damping * _velocity;
        Vector3 acceleration = force / mass;

        _velocity += acceleration * Time.deltaTime;
        _currentPosition += _velocity * Time.deltaTime;

        transform.localPosition = _initialLocalPosition + _currentPosition;
    }

    private void ApplyLandingForce()
    {
        float fallSpeed = Mathf.Abs(_lastVerticalVelocity);

        if (fallSpeed < 1.5f) return;

        float impactPower = Mathf.Min(fallSpeed * forceMultiplier, maxForce);

        _velocity.y -= impactPower;

        _lastVerticalVelocity = 0;
    }
}