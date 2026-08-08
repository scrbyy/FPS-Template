using Cysharp.Threading.Tasks;

public class Knife : Weapon
{
    public override void Deinitialize()
    {
        base.Deinitialize();
        _weaponAttacker.Deinitialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        _weaponAttacker = new KnifeAttacker(_data, _origin);
        OpenDelay(_data.OpenDelay).Forget();
        _weaponAttacker.Initialize();
    }
}