using UnityEngine;

public class GunSound : WeaponSound
{
    [SerializeField] protected Sound _reloadSound;

    [Header("References")]
    [SerializeField] private Gun _gun;

    private System.Action _playReloadSound;

    protected override void Awake()
    {
        base.Awake();
        Weapon = _gun;
        _playReloadSound = () => PlaySound(_reloadSound);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _gun.OnReloadStart += _playReloadSound;
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        _gun.OnReloadStart -= _playReloadSound;
    }
}