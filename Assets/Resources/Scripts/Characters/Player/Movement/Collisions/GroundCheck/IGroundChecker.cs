using System;

public interface IGroundChecker
{
    event Action OnGrounded;

    bool IsGrounded { get; }
}