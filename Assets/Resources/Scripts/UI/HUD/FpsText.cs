using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class FpsText : MonoBehaviour
{
    private TMP_Text _fpsText;

    [SerializeField] private float _updateInterval;

    private float _accumulatedTime = 0f;
    private int _frameCount = 0;

    private void Start()
    {
        _fpsText = GetComponent<TMP_Text>();
        _fpsText.text = "{0} FPS";
    }

    private void Update()
    {
        _accumulatedTime += Time.unscaledDeltaTime;
        _frameCount++;

        if (_accumulatedTime >= _updateInterval)
        {
            int fps = Mathf.RoundToInt(_frameCount / _accumulatedTime);

            _fpsText.SetText("{0} FPS", fps);

            _accumulatedTime = 0f;
            _frameCount = 0;
        }
    }
}