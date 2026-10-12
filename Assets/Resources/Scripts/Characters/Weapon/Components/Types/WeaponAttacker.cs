using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

public abstract class WeaponAttacker
{
    public event Action OnAttack;
    public event Action<HitData> OnHit;

    public bool IsAutoAttacking => _isAutoAttacking;
    protected bool _isAutoAttacking;

    protected readonly IAttackData _attackData;
    protected IAttackMethod _attackMethod;
    protected HitHandler _hitHandler;

    protected CancellationTokenSource _attackCts;
    protected float _lastAttackTime;

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

    public virtual async UniTask StartAttack()
    {
        if (_isAutoAttacking) return;

        ResetCts();
        _isAutoAttacking = true;

        try
        {
            float interval = _attackData.AfterAttackDelay;
            float nextAttackTime = Time.time;

            while (_isAutoAttacking && CanAttack())
            {
                float delay = nextAttackTime - Time.time;

                if (delay > 0)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: _attackCts.Token);
                }

                if (!_isAutoAttacking || !CanAttack()) break;

                SingleAttack();

                nextAttackTime += interval;

                if (nextAttackTime < Time.time)
                {
                    nextAttackTime = Time.time;
                }
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _isAutoAttacking = false;
        }
    }

    public void StopAttack()
    {
        if (_isAutoAttacking)
        {
            _isAutoAttacking = false;
            _attackCts?.Cancel();
        }
    }

    public virtual void Initialize()
    {
        _attackCts = new CancellationTokenSource();
    }

    public virtual void Deinitialize()
    {
        StopAttack();
        _attackCts?.Dispose();
    }

    protected virtual bool CanAttack() => true;

    protected virtual float CalculateDamage(HitData hitData) => _attackData.Damage;

    protected virtual void SingleAttack()
    {
        _lastAttackTime = Time.time;

        OnAttack?.Invoke();
        HitData hitData = _attackMethod.Execute();

        if (hitData.IsHit)
        {
            OnHit?.Invoke(hitData);
            _hitHandler.HandleHit(hitData, CalculateDamage(hitData));
        }
    }

    protected void ResetCts()
    {
        _attackCts?.Cancel();
        _attackCts?.Dispose();
        _attackCts = new CancellationTokenSource();
    }
}