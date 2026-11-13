using System;
using UnityEngine;

public interface IGroundChecker
{
    event Action OnGrounded;

    bool IsGrounded { get; }
}