using UnityEngine;
using System.Collections.Generic;

namespace scrbyy.FpsTemplate.Characters.Player.CameraSystem.Effects
{
    public class PositionEffectHandler : MonoBehaviour
    {
        [SerializeField] private List<PositionEffect> _effects = new List<PositionEffect>();

        private Vector3 _initialLocalPosition;

        private void Awake()
        {
            _initialLocalPosition = transform.localPosition;
        }

        private void LateUpdate()
        {
            Vector3 totalOffset = Vector3.zero;

            foreach (var effect in _effects)
            {
                totalOffset += effect.GetLocalOffset();
            }

            transform.localPosition = _initialLocalPosition + totalOffset;
        }
    }
}