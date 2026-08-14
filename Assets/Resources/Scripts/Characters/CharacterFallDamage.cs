using UnityEngine;
using Zenject;

public class CharacterFallDamage : MonoBehaviour
{
    [Header("Clamping")]
    [SerializeField, Range(-20, 0)] private float _minDamagableSpeed;

    [Header("Formula")]
    [SerializeField] private AnimationCurve _damageCurve;

    [Header("References")]
    [SerializeField] private CharacterEngine _characterEngine;
    [SerializeField] private CharacterHealth _attachedHealth;

    [Inject] private IGroundChecker _groundChecker;

    private float _maxFallSpeed;

    private void Update()
    {
        if(_groundChecker.IsGrounded == false && _characterEngine.Velocity.y < 0 && _maxFallSpeed > _characterEngine.Velocity.y)
        {
            _maxFallSpeed = _characterEngine.Velocity.y;
        }
    }

    private void DoFallDamage()
    {
        if (_maxFallSpeed > _minDamagableSpeed) return;
        int damage = Mathf.RoundToInt(_damageCurve.Evaluate(-_maxFallSpeed));
        _attachedHealth.TakeDamage(damage);
        _maxFallSpeed = 0f;
    }
    private void OnEnable()
    {
        _groundChecker.OnGrounded += DoFallDamage;
    }

    private void OnDisable()
    {
        _groundChecker.OnGrounded -= DoFallDamage;
    }
}