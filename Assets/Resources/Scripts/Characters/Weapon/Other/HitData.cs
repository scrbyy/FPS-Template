using UnityEngine;

public struct HitData
{
    public Vector3 originPoint;
    public Vector3 hitPoint;
    public bool isHit;
    public GameObject hitObject;

    public float Distance => Vector3.Distance(originPoint, hitPoint);
}