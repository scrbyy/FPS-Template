using UnityEngine;

namespace scrbyy.FpsTemplate.Characters.Player.Camera.Effects
{
    public abstract class RotationEffect : MonoBehaviour
    {
        public abstract Quaternion GetLocalRotationOffset();
    }
}