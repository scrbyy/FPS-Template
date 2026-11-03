using UnityEngine;
using Zenject;

public class AttackMethodFactory
{
    private readonly DiContainer _container;

    public AttackMethodFactory(DiContainer container)
    {
        _container = container;
    }

    public IAttackMethod Create(AttackMethodData data, Transform origin)
    {
        return data.CreateMethod(origin, _container);
    }
}