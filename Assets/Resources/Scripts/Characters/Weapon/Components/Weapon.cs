using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public Action OnAttack;
    public Action<HitData> OnShotContact;

    [SerializeField] protected CharacterSpeed _ownerSpeedHandler;

    protected float _damage;
    protected bool _isOpen;
    protected CancellationTokenSource _openCts;

    public abstract void Attack();

    public abstract void Initialize();

    public abstract void Deinitialize();

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