using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public event Action OnAttack;
    public event Action OnAttackStopped;
    public event Action<HitData> OnShotContact;

    public FireMode FireMode => _data.FireMode;

    [SerializeField] protected WeaponData _data;
    [SerializeField] protected Transform _origin;
    [SerializeField] protected CharacterSpeed _ownerSpeedHandler;

    protected bool _isOpen;
    protected CancellationTokenSource _openCts;
    protected WeaponSpeedModifier _speedModifier;

    protected WeaponAttacker _weaponAttacker;

    public virtual void Attack()
    {
        if (!_isOpen) return;
        _weaponAttacker?.StartShoot().Forget();
    }

    public virtual void StopAttack()
    {
        _weaponAttacker?.StopShoot();
        NotifyAttackStopped();
    }

    public virtual void Initialize()
    {
        _isOpen = false;
        _openCts = new CancellationTokenSource();

        OpenDelay(_data.OpenDelay).Forget();

        _speedModifier = new WeaponSpeedModifier(_data.SpeedMultiplier);
        _ownerSpeedHandler.AddModifier(_speedModifier);
    }

    public virtual void Deinitialize()
    {
        if (_openCts != null)
        {
            _openCts.Cancel();
            _openCts.Dispose();
            _openCts = null;
        }

        _isOpen = false;
        _ownerSpeedHandler.RemoveModifier(_speedModifier);
    }

    public async UniTask OpenDelay(float openTime)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(openTime), cancellationToken: _openCts.Token);
            _isOpen = true;
        }
        catch (OperationCanceledException) { }
    }

    protected void NotifyAttack() => OnAttack?.Invoke();
    protected void NotifyAttackStopped() => OnAttackStopped?.Invoke();
    protected void NotifyShotContact(HitData hitData) => OnShotContact?.Invoke(hitData);
}