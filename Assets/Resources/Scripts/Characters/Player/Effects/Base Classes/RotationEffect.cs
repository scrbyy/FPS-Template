using UnityEngine;

public abstract class RotationEffect : MonoBehaviour
{
    public abstract Quaternion GetLocalRotationOffset();
}