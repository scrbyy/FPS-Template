using UnityEngine;

public class FallSpringEffect : MonoBehaviour, IMotionEffect
{
    [Header("Spring Settings")]
    [SerializeField] private float stiffness = 100f;
    [SerializeField] private float damping = 10f;
    [SerializeField] private float mass = 1f;

    [Header("Impact Force")]
    [SerializeField] private float forceMultiplier = 0.2f;
    [SerializeField] private float maxForce = 2f;

    [Header("References")]
    [SerializeField] private PlayerEngine _playerEngine;

    private Vector3 _currentPosition;
    private Vector3 _velocity;
    private float _lastVerticalVelocity;

    public Vector3 GetLocalOffset() => _currentPosition;

    private void OnEnable() => _playerEngine.OnLanded += ApplyLandingForce;
    private void OnDisable() => _playerEngine.OnLanded -= ApplyLandingForce;

    private void Update()
    {
        float currentYVel = _playerEngine.GetVelocity().y;
        if (currentYVel < -0.1f)
            _lastVerticalVelocity = currentYVel;
    }

    private void LateUpdate()
    {
        Vector3 force = -stiffness * _currentPosition - damping * _velocity;
        Vector3 acceleration = force / mass;

        _velocity += acceleration * Time.deltaTime;
        _currentPosition += _velocity * Time.deltaTime;
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