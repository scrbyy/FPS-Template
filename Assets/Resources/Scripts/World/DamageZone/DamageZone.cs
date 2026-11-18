using System;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;

using scrbyy.FpsTemplate.Characters.Damage;


namespace scrbyy.FpsTemplate.World.DamageZone
{
    [RequireComponent(typeof(Collider))]
    public class DamageZone : MonoBehaviour
    {
        [SerializeField] private int _damageAmount;
        [SerializeField] private DamageZoneType _damageZoneType;

        [Header("Repeatable damage settings")]
        [SerializeField] private float _repeatTime;

        private bool _inZone;
        private CancellationTokenSource _damageCts;

        private async UniTaskVoid RepeatableDamaging(IDamageable target, CancellationToken token)
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
            if (other.TryGetComponent<IDamageable>(out IDamageable target))
            {
                if (_damageZoneType == DamageZoneType.Instant)
                {
                    target.TakeDamage(_damageAmount);
                }

                else if (_damageZoneType == DamageZoneType.Repeatable)
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
            if (other.GetComponent<IDamageable>() != null)
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
}