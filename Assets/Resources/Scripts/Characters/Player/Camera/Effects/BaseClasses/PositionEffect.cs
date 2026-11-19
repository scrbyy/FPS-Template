using UnityEngine;

namespace scrbyy.FpsTemplate.Characters.Player.Camera.Effects
{
    public abstract class PositionEffect : MonoBehaviour
    {
        public abstract Vector3 GetLocalOffset();
    }
}