using UnityEngine;

namespace scrbyy.FpsTemplate.InputSystem
{
    public interface ILookInputProvider
    {
        public Vector2 LookInput { get; }
    }
}