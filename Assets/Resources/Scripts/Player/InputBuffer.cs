using UnityEngine;
public class InputBuffer
{

    private float bufferDuration;
    private float lastInputTime = float.NegativeInfinity;

    public InputBuffer(float duration)
    {
        bufferDuration = duration;
    }

    public void SetBuffer()
    {
        lastInputTime = Time.time;
    }

    public bool HasBuffer()
    {
        return Time.time < lastInputTime + bufferDuration;
    }

    public void ConsumeBuffer()
    {
        lastInputTime = float.NegativeInfinity;
    }
}