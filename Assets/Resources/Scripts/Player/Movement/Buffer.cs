using UnityEngine;
public class Buffer
{

    private float bufferDuration;
    private float lastInputTime = float.NegativeInfinity;

    public Buffer(float duration)
    {
        bufferDuration = duration;
    }

    public void Set()
    {
        lastInputTime = Time.time;
    }

    public bool Has()
    {
        return Time.time < lastInputTime + bufferDuration;
    }

    public void Reset()
    {
        lastInputTime = float.NegativeInfinity;
    }
}