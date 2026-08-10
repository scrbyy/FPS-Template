using UnityEngine;

[System.Serializable]
public abstract class RotationEffect : MonoBehaviour
{
    public abstract Quaternion GetLocalRotationOffset();
}