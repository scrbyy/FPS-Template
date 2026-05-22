using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    [Header("Positions")]
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    [Header("Recoil Settings per Shot")]
    [SerializeField] private Vector3 _recoilForce;

    [SerializeField] private Vector3 _recoilRandomness;

    [Header("Limits and Clamping")]
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

        targetPosition += finalRecoil;

        targetPosition.x = Mathf.Clamp(targetPosition.x, initialPosition.x + _minBounds.x, initialPosition.x + _maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, initialPosition.y + _minBounds.y, initialPosition.y + _maxBounds.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, initialPosition.z + _minBounds.z, initialPosition.z + _maxBounds.z);

        _shotCount++;
    }

    private void Start()
    {
        initialPosition = transform.localPosition;
        currentPosition = initialPosition;
        targetPosition = initialPosition;
    }

    private void Update()
    {
        if (Time.time - _lastShotTime > _resetTime)
        {
            _shotCount = 0;
        }

        targetPosition = Vector3.Lerp(targetPosition, initialPosition, _returnSpeed * Time.deltaTime);

        currentPosition = Vector3.Lerp(currentPosition, targetPosition, _snappiness * Time.deltaTime);
        transform.localPosition = currentPosition;
    }

    private void OnEnable()
    {
        _gun.OnAttack += FireRecoil;
    }

    private void OnDisable()
    {
        _gun.OnAttack += FireRecoil;
    }
}