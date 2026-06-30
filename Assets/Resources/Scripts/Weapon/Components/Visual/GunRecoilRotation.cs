using UnityEngine;

public class GunRecoilRotation : MonoBehaviour
{
    [Header("Rotations (Euler Angles)")]
    private Vector3 initialRotation;
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    [Header("Recoil Settings per Shot")]
    [SerializeField] private Vector3 _recoilForce;
    [SerializeField] private Vector3 _recoilRandomness;

    [Header("Limits and Clamping (Relative to Initial)")]
    [SerializeField] private Vector3 _minBounds;
    [SerializeField] private Vector3 _maxBounds;

    [Header("Return Speed and Snappiness")]
    [SerializeField] private float _returnSpeed;
    [SerializeField] private float _snappiness;

    [Header("Diminishing Dynamics")]
    [SerializeField] private float _recoilDecreaseFactor;
    [SerializeField] private float _minRecoilMultiplier;
    [SerializeField] private float _resetTime;

    [Header("References")]
    [SerializeField] private Gun _gun;

    private int _shotCount;
    private float _lastShotTime;

    public void FireRecoil()
    {
        _lastShotTime = Time.time;

        float currentMultiplier = Mathf.Pow(_recoilDecreaseFactor, _shotCount);
        currentMultiplier = Mathf.Max(currentMultiplier, _minRecoilMultiplier);

        Vector3 randomOffset = new Vector3(
            Random.Range(-_recoilRandomness.x, _recoilRandomness.x),
            Random.Range(-_recoilRandomness.y, _recoilRandomness.y),
            Random.Range(-_recoilRandomness.z, _recoilRandomness.z)
        );

        Vector3 finalRecoil = (_recoilForce + randomOffset) * currentMultiplier;

        targetRotation += finalRecoil;

        targetRotation.x = Mathf.Clamp(targetRotation.x, initialRotation.x + _minBounds.x, initialRotation.x + _maxBounds.x);
        targetRotation.y = Mathf.Clamp(targetRotation.y, initialRotation.y + _minBounds.y, initialRotation.y + _maxBounds.y);
        targetRotation.z = Mathf.Clamp(targetRotation.z, initialRotation.z + _minBounds.z, initialRotation.z + _maxBounds.z);

        _shotCount++;
    }

    private void Start()
    {
        initialRotation = transform.localEulerAngles;

        initialRotation = NormalizeAngles(initialRotation);

        currentRotation = initialRotation;
        targetRotation = initialRotation;
    }

    private void Update()
    {
        if (Time.time - _lastShotTime > _resetTime)
        {
            _shotCount = 0;
        }

        targetRotation = Vector3.Lerp(targetRotation, initialRotation, _returnSpeed * Time.deltaTime);

        currentRotation = Vector3.Lerp(currentRotation, targetRotation, _snappiness * Time.deltaTime);

        transform.localRotation = Quaternion.Euler(currentRotation);
    }

    private void OnEnable()
    {
        if (_gun != null) _gun.OnAttack += FireRecoil;
    }

    private void OnDisable()
    {
        if (_gun != null) _gun.OnAttack -= FireRecoil;
    }

    private Vector3 NormalizeAngles(Vector3 angles)
    {
        angles.x = NormalizeAngle(angles.x);
        angles.y = NormalizeAngle(angles.y);
        angles.z = NormalizeAngle(angles.z);
        return angles;
    }

    private float NormalizeAngle(float angle)
    {
        while (angle > 180f) angle -= 360f;
        while (angle < -180f) angle += 360f;
        return angle;
    }
}