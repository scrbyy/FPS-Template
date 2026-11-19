using UnityEngine;

namespace scrbyy.FpsTemplate.Characters.Player.CameraSystem.Effects
{
    public abstract class RotationEffect : MonoBehaviour
    {
        public abstract Quaternion GetLocalRotationOffset();
    }
}