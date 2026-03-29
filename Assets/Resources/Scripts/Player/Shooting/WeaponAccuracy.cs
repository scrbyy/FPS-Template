using UnityEngine;

public class WeaponAccuracy : MonoBehaviour, IAccurcacy
{
    [SerializeField] private Vector2 _verticalAccuracyAngle;
    [SerializeField] private Vector2 _horizonatalAccuracyAngle;

    private float _accuracyModifier = 1;

    public float GetHorizontalAccuracyAngle()
    {
        throw new System.NotImplementedException();
    }

    public float GetVerticalAccuracyAngle()
    {
        return Random.Range(_verticalAccuracyAngle.x, _verticalAccuracyAngle.y) * _accuracyModifier;
    }
}
