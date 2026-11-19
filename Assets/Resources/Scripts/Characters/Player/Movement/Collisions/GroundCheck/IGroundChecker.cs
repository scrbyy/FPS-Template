using System;

namespace scrbyy.FpsTemplate.Characters.Player.Movement.GroundChecker
{
    public interface IGroundChecker
    {
        event Action OnGrounded;

        bool IsGrounded { get; }
    }
}