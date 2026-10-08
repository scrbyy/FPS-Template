using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class WeaponAttacker
{
    public virtual event Action OnShoot;
    public virtual event Action<HitData> OnShotContact;

    public bool IsAttacking => _isAttacking;
    protected bool _isAttacking;

    protected readonly IAttackData _attackData;
    protected IAttackMethod _attackMethod;
    protected HitHandler _hitHandler;

    protected CancellationTokenSource _shootCts;

    // Поле для отслеживания времени последнего выстрела
    protected float _lastShootTime;

    public WeaponAttacker(
            AttackData attackConfig,
            Transform origin,
            IAttackData attackData,
            AttackMethodFactory attackFactory)
    {
        _attackData = attackData;
        _attackMethod = attackFactory.Create(attackConfig, origin);
        _hitHandler = new HitHandler();
    }

    protected virtual void Shoot()
    {
        _lastShootTime = Time.time;

        OnShoot?.Invoke();
        HitData hitData = _attackMethod.Execute();

        if (hitData.IsHit)
        {
            OnShotContact?.Invoke(hitData);
            _hitHandler.HandleShot(hitData, _attackData.Damage);
        }
    }

    public virtual void Initialize()
    {
        _shootCts = new CancellationTokenSource();
    }

    public virtual void Deinitialize()
    {
        StopShoot();
        _shootCts?.Dispose();
    }

    public virtual async UniTask StartShoot()
    {
        if (_isAttacking) return;

        ResetCts();
        _isAttacking = true;

        try
        {
            while (_isAttacking)
            {
                float timeSinceLastShoot = Time.time - _lastShootTime;
                float remainingDelay = _attackData.AfterAttackDelay - timeSinceLastShoot;

                if (remainingDelay > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(remainingDelay), cancellationToken: _shootCts.Token);
                }

                if (!_isAttacking) break;

                Shoot();

                await UniTask.Delay(TimeSpan.FromSeconds(_attackData.AfterAttackDelay), cancellationToken: _shootCts.Token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isAttacking = false;
        }
    }

    public void StopShoot()
    {
        if (_isAttacking)
        {
            _isAttacking = false;
            _shootCts?.Cancel();
        }
    }

    protected void ResetCts()
    {
        _shootCts?.Cancel();
        _shootCts?.Dispose();
        _shootCts = new CancellationTokenSource();
    }

    public virtual void StartAttack() { }

    public virtual void StopAttack() { }
}