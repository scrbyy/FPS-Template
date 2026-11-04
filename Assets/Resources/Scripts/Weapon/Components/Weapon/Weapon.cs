using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class Weapon : MonoBehaviour
{
    public event Action OnOpening;
    public event Action OnAttack;
    public event Action OnAttackStopped;
    public event Action<HitData> OnContact;

    public FireMode FireMode => _data.FireMode;

    [SerializeField] protected WeaponData _data;
    [SerializeField] protected Transform _origin;
    [SerializeField] protected CharacterSpeed _ownerSpeedHandler;

    protected CancellationTokenSource _openCts;
    protected WeaponSpeedModifier _speedModifier;
    protected WeaponAttacker _weaponAttacker;

    protected bool _isOpened;

    public virtual void Initialize()
    {
        _isOpened = false;
        _openCts = new CancellationTokenSource();

        OnOpening?.Invoke();

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

        _isOpened = false;
        _ownerSpeedHandler.RemoveModifier(_speedModifier);
    }

    public virtual void Attack()
    {
        if (!_isOpened) return;
        _weaponAttacker?.StartAttack().Forget();
    }

    public virtual void StopAttack()
    {
        _weaponAttacker?.StopAttack();
        NotifyAttackStopped();
    }

    protected async UniTask OpenDelay(float openTime)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(openTime), cancellationToken: _openCts.Token);
            _isOpened = true;
        }
        catch (OperationCanceledException) { }
    }

    protected void SubscribeAttackerEvents()
    {
        if (_weaponAttacker != null)
        {
            _weaponAttacker.OnAttack += NotifyAttack;
            _weaponAttacker.OnHit += HandleAttackerHit;
        }
    }

    protected void UnsubscribeAttackerEvents()
    {
        if (_weaponAttacker != null)
        {
            _weaponAttacker.OnAttack -= NotifyAttack;
            _weaponAttacker.OnHit -= HandleAttackerHit;
        }
    }

    protected void HandleAttackerHit(HitData hitData)
    {
        NotifyShotContact(hitData);
    }

    protected void NotifyAttack() => OnAttack?.Invoke();

    protected void NotifyAttackStopped() => OnAttackStopped?.Invoke();

    protected void NotifyShotContact(HitData hitData) => OnContact?.Invoke(hitData);
}