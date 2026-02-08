using UnityEngine;

public class LandingPhysics : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerJump playerJump;
    [SerializeField] private PlayerEngine playerEngine;

    [Header("Spring Settings")]
    [SerializeField] private float stiffness = 150f;
    [SerializeField] private float damping = 15f;
    [SerializeField] private float mass = 1f;

    [Header("Impact Force")]
    [SerializeField] private float forceMultiplier = 0.15f;
    [SerializeField] private float maxForce = 4f;

    private Vector3 _currentPosition;
    private Vector3 _velocity;
    private Vector3 _initialLocalPosition;
    private float _lastVerticalVelocity;

    private void Awake() => _initialLocalPosition = transform.localPosition;

    private void OnEnable()
    {
        if (playerJump != null) playerJump.OnLanded += ApplyLandingForce;
    }

    private void OnDisable()
    {
        if (playerJump != null) playerJump.OnLanded -= ApplyLandingForce;
    }

    private void Update()
    {
        // Запоминаем скорость ПЕРЕД приземлением, так как в момент касания она станет 0
        float currentYVel = playerJump.GetVerticalVelocity();
        if (currentYVel < -0.1f)
            _lastVerticalVelocity = currentYVel;
    }

    private void LateUpdate()
    {
        // Математика пружины
        Vector3 force = -stiffness * _currentPosition - damping * _velocity;
        Vector3 acceleration = force / mass;

        _velocity += acceleration * Time.deltaTime;
        _currentPosition += _velocity * Time.deltaTime;

        // Двигаем только этот объект
        transform.localPosition = _initialLocalPosition + _currentPosition;
    }

    private void ApplyLandingForce()
    {
        float fallSpeed = Mathf.Abs(_lastVerticalVelocity);

        if (fallSpeed < 1.5f) return; // Порог срабатывания

        float impactPower = Mathf.Min(fallSpeed * forceMultiplier, maxForce);

        // Импульс вниз
        _velocity.y -= impactPower;

        // Сбрасываем накопленную скорость падения после удара
        _lastVerticalVelocity = 0;
    }
}