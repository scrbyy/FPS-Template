using System;
using UnityEngine;

public abstract class GroundChecker : MonoBehaviour
{
    public abstract event Action OnGrounded;

    public abstract bool IsGrounded { get; }
}