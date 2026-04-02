using UnityEngine;

public class WeaponAccuracy : MonoBehaviour
{
    [Header("Vertical Clamping")]
    [SerializeField] private float _maxVerticalAccuracy;
    [SerializeField] private float _minVerticalAccuracy;

    [Header("Horizontal Clamping")]
    [SerializeField] private float _maxHorizonatalAccuracy;
    [SerializeField] private float _minHorizonatalAccuracy;

    [Space]
    [SerializeField] private float _accuracyModifier;

    public Vector2 GetAccurcacyVector()
    {
        return new Vector2(Random.Range(_minHorizonatalAccuracy, _maxHorizonatalAccuracy), Random.Range(_minVerticalAccuracy, _maxVerticalAccuracy));
    }
}
