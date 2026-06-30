using System;

public interface IGroundChecker
{
    public event Action OnGrounded;

    public bool IsGrounded { get; }
}