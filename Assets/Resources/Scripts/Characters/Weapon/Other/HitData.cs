using UnityEngine;

public struct HitData
{
    public Vector3 origin;
    public Vector3 hit;
    public bool isHit;
    public GameObject gameobject;
    public Vector3 normal;

    public float Distance => Vector3.Distance(origin, hit);

    public void SetData(RaycastHit raycast)
    {
        hit = raycast.point;
        gameobject = raycast.transform.gameObject;
        normal = raycast.normal;
    }
}