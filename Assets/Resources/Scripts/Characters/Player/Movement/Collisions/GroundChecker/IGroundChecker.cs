using System;

namespace scrbyy.FpsTemplate.Characters.Player.Movement.Collisions.GroundChecker
{
    public interface IGroundChecker
    {
        event Action OnGrounded;

        bool IsGrounded { get; }
    }
}