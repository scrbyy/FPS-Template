using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Action OnAttack;
    public Action OnStopAttack;
    public Action<HitData> OnShotContact;

    public FireMode FireMode => _data.FireMode;

    [SerializeField] protected WeaponData _data;

    [SerializeField] protected Transform _origin;
    [SerializeField] protected CharacterSpeed _ownerSpeedHandler;

    protected bool _isOpen;
    protected CancellationTokenSource _openCts;
    protected WeaponSpeedModifier _speedModifier;

    public abstract void Attack();
    public abstract void StopAttack();

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

    public async UniTask OpenDelay(float _openTime)
    {
        try
        {
            await UniTask.Delay(TimeSpan.FromSeconds(_openTime), cancellationToken: _openCts.Token);
            _isOpen = true;
        }
        catch (OperationCanceledException) { }
    }
}