using UnityEngine;

public class KnifeSound : WeaponSound
{
    [Header("References")]
    [SerializeField] private Knife _knife;

    protected override void Awake()
    {
        base.Awake();
        Weapon = _knife;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
}