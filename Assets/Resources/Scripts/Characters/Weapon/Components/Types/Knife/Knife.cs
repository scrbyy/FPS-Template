using Cysharp.Threading.Tasks;

public class Knife : Weapon
{
    private KnifeAttacker _knifeAttacker;

    public override void Attack()
    {
        if (_isOpen == false) return;
        _knifeAttacker.StartShoot().Forget();
    }

    public override void Deinitialize()
    {
        base.Deinitialize();
        _knifeAttacker.Deinitialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        _knifeAttacker = new KnifeAttacker(_data, _origin);
        OpenDelay(_data.OpenDelay).Forget();
        _knifeAttacker.Initialize();
    }

    public override void StopAttack()
    {
        _knifeAttacker?.StopShoot();
        OnStopAttack?.Invoke();
    }
}