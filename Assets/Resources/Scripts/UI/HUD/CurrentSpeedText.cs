using TMPro;
using UnityEngine;

public class CurrentSpeedText : MonoBehaviour
{
    [SerializeField] private TMP_Text _speedText;
    [SerializeField] private CharacterEngine _characterEngine;
    [SerializeField] private float _updateInterval;

    private float _accumulatedTime;

    private void Update()
    {
        _accumulatedTime += Time.unscaledDeltaTime;

        if (_accumulatedTime >= _updateInterval)
        {
            _accumulatedTime -= _updateInterval;

            Vector3 vel = _characterEngine.Velocity;

            float speed = Mathf.Sqrt(vel.x * vel.x + vel.z * vel.z);

            _speedText.SetText("Current Velocity: {0:1}", speed);
        }
    }
}