using UnityEngine;

public struct HitData
{
    public Vector3 Origin;
    public Vector3 HitPoint;
    public bool IsHit;
    public GameObject GameObject;
    public Vector3 Normal;

    public float Distance => Vector3.Distance(Origin, HitPoint);

    public void SetData(RaycastHit raycast)
    {
        HitPoint = raycast.point;
        GameObject = raycast.transform.gameObject;
        Normal = raycast.normal;
    }
}