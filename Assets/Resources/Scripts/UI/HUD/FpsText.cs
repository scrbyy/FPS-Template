using UnityEngine;

public class FpsText : UpdateableText
{
    private int _frameCount;

    protected override void Start()
    {
        base.Start();
        _text.SetText("{0} FPS", 0);
    }

    private void LateUpdate()
    {
        _frameCount++;
    }

    protected override void UpdateText()
    {
        float time = _updateInterval > 0 ? _updateInterval : Time.unscaledDeltaTime;
        int fps = Mathf.RoundToInt(_frameCount / time);

        _text.SetText("{0} FPS", fps);

        _frameCount = 0;
    }
}