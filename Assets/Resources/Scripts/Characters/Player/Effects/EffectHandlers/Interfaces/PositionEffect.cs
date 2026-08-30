using UnityEngine;

[System.Serializable]
public abstract class PositionEffect : MonoBehaviour
{
    public abstract Vector3 GetLocalOffset();
}