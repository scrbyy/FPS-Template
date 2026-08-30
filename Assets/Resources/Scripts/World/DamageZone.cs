using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Collider))]

public class DamageZone : MonoBehaviour
{
    [SerializeField] private int _damageAmount;
    [SerializeField] private DamageType _damageType;
    [Header("Repeatable damage settings")]
    [SerializeField] private float _repeatTime;

    private CancellationTokenSource _damageCts;

    private bool _inZone;

    private async UniTaskVoid RepeatableDamaging(IDamagable target, CancellationToken token)
    {
        try
        {
            while (_inZone)
            {
                target.TakeDamage(_damageAmount);

                await UniTask.Delay(TimeSpan.FromSeconds(_repeatTime), cancellationToken: token);
            }
        }
        catch (OperationCanceledException) { }
        finally
        {
            _inZone = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<IDamagable>(out IDamagable target))
        {
            if (_damageType == DamageType.Instant)
            {
                target.TakeDamage(_damageAmount);
            }

            else if (_damageType == DamageType.Repeatable)
            {
                StopDamaging();

                _inZone = true;
                _damageCts = new CancellationTokenSource();

                RepeatableDamaging(target, _damageCts.Token).Forget();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<IDamagable>() != null)
        {
            StopDamaging();
        }
    }

    private void StopDamaging()
    {
        _inZone = false;

        if (_damageCts != null)
        {
            _damageCts.Cancel();
            _damageCts.Dispose();
            _damageCts = null;
        }
    }

    private void OnDestroy()
    {
        StopDamaging();
    }
}