using UnityEngine;

namespace scrbyy.FpsTemplate.Services
{
    public class Buffer
    {
        private float _duration;
        private float _lastInputTime = float.NegativeInfinity;

        public Buffer(float duration)
        {
            _duration = duration;
        }

        public void Set()
        {
            _lastInputTime = Time.time;
        }

        public bool Has()
        {
            return Time.time < _lastInputTime + _duration;
        }

        public void Reset()
        {
            _lastInputTime = float.NegativeInfinity;
        }
    }
}