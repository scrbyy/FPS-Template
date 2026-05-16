using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{
    [Header("Positions")]
    private Vector3 initialPosition;
    private Vector3 currentPosition;
    private Vector3 targetPosition;

    [Header("Recoil Settings per Shot")]
    [Tooltip("Base displacement force along X (left/right), Y (up/down), Z (back/forward) axes")]
    [SerializeField] private Vector3 recoilForce = new Vector3(0.02f, 0.05f, -0.1f);

    [Tooltip("Random offset applied to each axis to create unpredictable spray")]
    [SerializeField] private Vector3 recoilRandomness = new Vector3(0.03f, 0.02f, 0.02f);

    [Header("Limits and Clamping (Relative to Initial Position)")]
    [SerializeField] private Vector3 minBounds = new Vector3(-0.1f, -0.02f, -0.25f);
    [SerializeField] private Vector3 maxBounds = new Vector3(0.1f, 0.15f, 0.05f);

    [Header("Return Speed and Snappiness")]
    [SerializeField] private float returnSpeed = 10f;
    [SerializeField] private float snappiness = 20f;

    [Header("Diminishing Dynamics (CS-Style)")]
    [SerializeField] private float recoilDecreaseFactor = 0.85f;
    [SerializeField] private float minRecoilMultiplier = 0.3f;
    [SerializeField] private float resetTime = 0.25f;

    [Header("References")]
    [SerializeField] private Weapon _weapon;

    private int shotCount = 0;
    private float lastShotTime;

    private void Start()
    {
        initialPosition = transform.localPosition;
        currentPosition = initialPosition;
        targetPosition = initialPosition;

        if (_weapon != null)
        {
            _weapon.OnWeaponShoot += FireRecoil;
        }
        else
        {
            Debug.LogError($"Weapon reference is missing on {gameObject.name}!", this);
        }
    }

    private void OnDestroy()
    {
        if (_weapon != null)
        {
            _weapon.OnWeaponShoot -= FireRecoil;
        }
    }

    private void Update()
    {
        if (Time.time - lastShotTime > resetTime)
        {
            shotCount = 0;
        }

        targetPosition = Vector3.Lerp(targetPosition, initialPosition, returnSpeed * Time.deltaTime);

        currentPosition = Vector3.Lerp(currentPosition, targetPosition, snappiness * Time.deltaTime);
        transform.localPosition = currentPosition;
    }

    public void FireRecoil()
    {
        lastShotTime = Time.time;

        float currentMultiplier = Mathf.Pow(recoilDecreaseFactor, shotCount);
        currentMultiplier = Mathf.Max(currentMultiplier, minRecoilMultiplier);

        Vector3 randomOffset = new Vector3(
            Random.Range(-recoilRandomness.x, recoilRandomness.x),
            Random.Range(-recoilRandomness.y, recoilRandomness.y),
            Random.Range(-recoilRandomness.z, recoilRandomness.z)
        );

        Vector3 finalRecoil = (recoilForce + randomOffset) * currentMultiplier;

        targetPosition += finalRecoil;

        targetPosition.x = Mathf.Clamp(targetPosition.x, initialPosition.x + minBounds.x, initialPosition.x + maxBounds.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, initialPosition.y + minBounds.y, initialPosition.y + maxBounds.y);
        targetPosition.z = Mathf.Clamp(targetPosition.z, initialPosition.z + minBounds.z, initialPosition.z + maxBounds.z);

        shotCount++;
    }

    private void OnEnable()
    {
        _weapon.OnWeaponShoot += FireRecoil;
    }

    private void OnDisable()
    {
        _weapon.OnWeaponShoot += FireRecoil;
    }
}